﻿using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Warehouse.Silverlight.Auth;
using Warehouse.Silverlight.Data;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Infrastructure.Events;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class ProductEditViewModel : InteractionRequestValidationObject
    {
        private readonly IDataService dataService;
        private readonly IEventAggregator eventAggregator;

        private string id;
        private string name;
        private string size;
        private string k;
        private string priceOpt;
        private long priceRozn;
        private double weight;
        private string count;
        private string nd;
        private string length;

        private bool isSheet;
        private double[] sheetSizes;

        public ProductEditViewModel(IDataService dataService, IEventAggregator eventAggregator, IAuthStore authStore)
            :this(null, dataService, eventAggregator, authStore)
        {
        }

        public ProductEditViewModel(Product product, IDataService dataService,
            IEventAggregator eventAggregator, IAuthStore authStore)
        {
            this.dataService = dataService;
            this.eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand<ChildWindow>(Save);

            if (product == null)
            {
                product = new Product();
                IsSheetCheckBoxVisible = true;
                IsSheetCheckBoxEnabled = true;
            }
            else
            {
                IsSheetCheckBoxVisible = product.IsSheet;
                IsSheetCheckBoxEnabled = false;
            }

            ProductToProps(product);

            var token = authStore.LoadToken();
            if (token != null)
            {
                IsEditor = token.IsEditor();
                DenyPriceEdit = !token.IsAdmin();
            }
        }

        public ICommand SaveCommand { get; private set; }
        public bool IsEditor { get; private set; }
        public bool DenyPriceEdit { get; private set; }
        public bool IsSheetCheckBoxVisible { get; private set; }
        public bool IsSheetCheckBoxEnabled { get; private set; }

        public string Title2
        {
            get
            {
                if (IsSheetCheckBoxEnabled)
                {
                    return isSheet ? "Новая позиция (лист)" : "Новая позиция";
                }
                return string.Format("{0} {1}", Name, Size);
            }
        }

        #region Name

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    ValidateName();
                }
            }
        }

        private void ValidateName()
        {
            errorsContainer.ClearErrors(() => Name);
            errorsContainer.SetErrors(() => Name, Validate.Required(Name));
        }

        #endregion

        #region Size

        public string Size
        {
            get { return size; }
            set
            {
                if (size != value)
                {
                    size = value;
                    ValidateSize();
                    if (isSheet)
                    {
                        UpdateSheetLength();
                    }
                }
            }
        }

        private void ValidateSize()
        {
            errorsContainer.ClearErrors(() => Size);
            errorsContainer.SetErrors(() => Size, Validate.Required(Size));
            if (isSheet)
            {
                sheetSizes = ParseSheetSize(size);
                if (sheetSizes == null)
                {
                    errorsContainer.SetErrors(() => Size, new[] { "строка в формате\nтолщина*ширина*длина" });
                }
            }
        }

        #endregion

        #region K

        public string K
        {
            get { return k; }
            set
            {
                if (k != value)
                {
                    k = value;
                    ValidateK();
                    UpdatePriceRozn();
                    UpdateWeight();
                }
            }
        }

        private void ValidateK()
        {
            errorsContainer.ClearErrors(() => K);
            errorsContainer.SetErrors(() => K, Validate.Double(K));
        }

        #endregion

        #region PriceOpt

        public string PriceOpt
        {
            get { return priceOpt; }
            set
            {
                if (priceOpt != value)
                {
                    priceOpt = value;
                    ValidatePriceOpt();
                    UpdatePriceRozn();
                }
            }
        }

        private void ValidatePriceOpt()
        {
            errorsContainer.ClearErrors(() => PriceOpt);
            errorsContainer.SetErrors(() => PriceOpt, Validate.Long(PriceOpt));
        }

        #endregion

        #region PriceRozn

        public long PriceRozn
        {
            get { return priceRozn; }
            set
            {
                if (priceRozn != value)
                {
                    priceRozn = value;
                    RaisePropertyChanged(() => PriceRozn);
                }
            }
        }

        private void UpdatePriceRozn()
        {
            if (errorsContainer.HasErrors)
            {
                PriceRozn = 0;
            }
            else
            {
                var _priceOpt = decimal.Parse(priceOpt);
                var _k = decimal.Parse(k);
                var rozn = _priceOpt * _k / 1000m * 1.2m;
                if (isSheet)
                {
                    var _l = decimal.Parse(length);
                    rozn *= _l;
                }
                PriceRozn = (long) (decimal.Ceiling(rozn / 100) * 100);
            }
        }

        #endregion

        #region Weight

        public double Weight
        {
            get { return weight; }
            set
            {
                if (Math.Abs(weight - value) > double.Epsilon)
                {
                    weight = value;
                    RaisePropertyChanged(() => Weight);
                }
            }
        }

        private void UpdateWeight()
        {
            if (errorsContainer.HasErrors)
            {
                Weight = 0;
            }
            else
            {
                var _count = int.Parse(count);
                var _length = double.Parse(length);
                var _nd = GetTotalNd();
                var _k = double.Parse(k);

                Weight = Math.Round( (_count * _length + _nd) * _k, 3);
            }
        }

        #endregion

        #region Count

        public string Count
        {
            get { return count; }
            set
            {
                if (count != value)
                {
                    count = value;
                    ValidateCount();
                    UpdateWeight();
                }
            }
        }

        private void ValidateCount()
        {
            errorsContainer.ClearErrors(() => Count);
            errorsContainer.SetErrors(() => Count, Validate.Int(Count));
        }

        #endregion

        #region Nd

        public string Nd
        {
            get { return nd; }
            set
            {
                if (nd != value)
                {
                    nd = value;
                    ValidateNd();
                    UpdateWeight();
                }
            }
        }

        private void ValidateNd()
        {
            errorsContainer.ClearErrors(() => Nd);
            if (!string.IsNullOrEmpty(nd))
            {
                var parts = nd.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var x in parts)
                {
                    var errors = Validate.Double(x).ToArray();
                    if (errors.Length > 0)
                    {
                        errorsContainer.SetErrors(() => Nd, new[] { "дробные числа, разделенные пробелом" });
                        break;
                    }
                }
            }
        }

        public string NdLabel
        {
            get { return isSheet ? "Н/Д (м²)" : "Н/Д (м)"; }
        }

        #endregion

        #region Length

        public string LenghtLabel
        {
            get { return isSheet ? "Площадь листа (м²)" : "Длина штанги (м)"; }
        }

        public string Length
        {
            get { return length; }
            set
            {
                if (length != value)
                {
                    length = value;
                    ValidateLength();
                    UpdateWeight();
                    RaisePropertyChanged(() => Length);
                }
            }
        }

        private void ValidateLength()
        {
            errorsContainer.ClearErrors(() => Length);
            errorsContainer.SetErrors(() => Length, Validate.Double(Length));
        }

        private void UpdateSheetLength()
        {
            if (errorsContainer.HasErrors || sheetSizes == null)
            {
                Length = "0";
            }
            else
            {
                var val = sheetSizes[1] / 1000 * sheetSizes[2] / 1000;
                Length = val.ToString("0.000");
            }
        }

        #endregion

        #region PriceIcome
        private string priceIcome;

        public string PriceIcome
        {
            get { return priceIcome; }
            set
            {
                if (priceIcome != value)
                {
                    priceIcome = value;
                    ValidatePriceIcome();
                }
            }
        }

        private void ValidatePriceIcome()
        {
            errorsContainer.ClearErrors(() => PriceIcome);
            errorsContainer.SetErrors(() => PriceIcome, Validate.Long(PriceIcome));
        }

        #endregion

        #region Internal

        public string Internal { get; set; }

        #endregion

        #region IsSheet

        public bool IsSheet
        {
            get { return isSheet; }
            set
            {
                isSheet = value;
                ValidateSize();
                UpdateSheetLength();
                UpdatePriceRozn();
                RaisePropertyChanged(() => IsSheet);
                RaisePropertyChanged(() => Title2);
                RaisePropertyChanged(() => LenghtLabel);
                RaisePropertyChanged(() => NdLabel);
            }
        }

        #endregion

        private async void Save(ChildWindow window)
        {
            ValidateName();
            ValidateSize();
            ValidateK();
            ValidatePriceOpt();
            ValidateCount();
            ValidateNd();
            ValidateLength();
            ValidatePriceIcome();

            if (HasErrors) return;

            var changed = PropsToProduct();

            var task = await dataService.SaveProductAsync(changed);
            if (task.Succeed)
            {
                var args = new ProductUpdatedEventArgs(task.Result);
                eventAggregator.GetEvent<ProductUpdatedEvent>().Publish(args);
                Confirmed = true;
                window.Close();
            }
        }

        private void ProductToProps(Product product)
        {
            id = product.Id;
            name = product.Name;
            size = product.Size;
            k = product.K.ToString("0.##");
            priceOpt = product.PriceOpt.ToString(CultureInfo.InvariantCulture);
            priceRozn = product.PriceRozn;
            weight = product.Weight;
            count = product.Count.ToString(CultureInfo.InvariantCulture);
            if (product.Nd != null)
            {
                nd = string.Join(" ", product.Nd);
            }
            length = product.Length.ToString("0.##");
            priceIcome = product.PriceIcome.ToString(CultureInfo.InvariantCulture);
            Internal = product.Internal;
            IsSheet = product.IsSheet;
        }

        private Product PropsToProduct()
        {
            return new Product
            {
                Id = id,
                Name = name,
                Size = size,
                K = Math.Round(double.Parse(k), 2),
                PriceOpt = long.Parse(priceOpt),
                PriceRozn = priceRozn,
                Weight = weight,
                Count = int.Parse(count),
                Nd = ParseNd(nd),
                Length = Math.Round(double.Parse(length), 2),
                PriceIcome = long.Parse(priceIcome),
                Internal = Internal,
                IsSheet = isSheet,
            };
        }

        private static double[] ParseNd(string nd)
        {
            return (nd ?? string.Empty)
                .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(double.Parse)
                .OrderByDescending(x => x)
                .ToArray();
        }

        private double GetTotalNd()
        {
            if (string.IsNullOrEmpty(nd))
            {
                return 0;
            }
            return nd.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).Sum();
        }

        private static double[] ParseSheetSize(string size)
        {
            if (size == null) return null;

            var parts = size.Split(new[] { ' ', '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return null;

            var sizes = parts[0].Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            if (sizes.Length != 3) return null;

            double d;
            if (sizes.All(x => double.TryParse(x, out d)))
            {
                return sizes.Select(double.Parse).ToArray();
            }

            return null;
        }
    }
}
