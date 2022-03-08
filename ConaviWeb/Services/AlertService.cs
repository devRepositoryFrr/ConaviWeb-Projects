using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Services
{
    public class AlertService
    {
        public static string ShowAlert(Alerts obj, string message)
        {
            string alertDiv = null;

            switch (obj)
            {
                case Alerts.Success:
                    alertDiv = "<div class='alertaOp alert alert-success' id='alerta'>" + message + ".</div>";
                    break;
                case Alerts.Danger:
                    alertDiv = "<div class='alertaOp alert alert-danger' id='alerta'>" + message + ".</div>";
                    break;
                case Alerts.Info:
                    alertDiv = "<div class='alertaOp alert alert-info' id='alerta'>" + message + ".</div>";
                    break;
                case Alerts.Warning:
                    alertDiv = "<div class='alertaOp alert alert-success' id='alerta'>" + message + ".</div>";
                    break;
            }

            return alertDiv;
        }
    }
}
