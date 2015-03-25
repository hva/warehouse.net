using System.Globalization;

namespace Warehouse.Silverlight.Controls.Behaviors
{
    public class CurrencyFormatTextBoxBehavior : ThrottledTextBindingBehavior
    {
        protected override void OnLoaded()
        {
            long l;
            if (long.TryParse(AssociatedObject.Text, NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out l))
            {
                // if valid long - applying format

                IsTextChangingLocked = true;

                AssociatedObject.Text = l.ToString("#,#");
                AssociatedObject.SelectionStart = AssociatedObject.Text.Length;

                IsTextChangingLocked = false;
            }
        }

        protected override void UpdateSource()
        {
            long l;
            if (long.TryParse(AssociatedObject.Text, NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out l))
            {
                // if valid long - applying format

                IsTextChangingLocked = true;

                AssociatedObject.Text = l.ToString(NumberFormatInfo.InvariantInfo);
                expression.UpdateSource();

                AssociatedObject.Text = l.ToString("#,#");
                AssociatedObject.SelectionStart = AssociatedObject.Text.Length;

                IsTextChangingLocked = false;
            }
            else
            {
                // value invalid - passing it through
                expression.UpdateSource();
            }
        }
    }
}
