using System;
using System.Diagnostics;
using System.Web;
using System.Windows;
using System.Windows.Extensions;

namespace Daybreak.Launch;
/// <summary>
/// Interaction logic for ExceptionDialog.xaml
/// </summary>
public partial class ExceptionDialog : Window
{
    private const string TitlePlaceholder = "[TITLE]";
    private const string BodyPlaceholder = "[BODY]";
    private const string IssueUrl = $"https://github.com/gwdevhub/Daybreak/issues/new?title={TitlePlaceholder}&body={BodyPlaceholder}&labels=bug";

    private string exceptionName;

    [GenerateDependencyProperty]
    private string exceptionMessage = string.Empty;

    public ExceptionDialog(Exception exception)
    {
        this.InitializeComponent();
        this.exceptionName = exception.GetType().Name;
        this.ExceptionMessage = exception.ToString();
    }

    public ExceptionDialog(string exceptionName, string exceptionMessage)
    {
        this.InitializeComponent();
        this.exceptionName = exceptionName;
        this.ExceptionMessage = exceptionMessage;
    }

    private void OkButton_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }

    private void ReportButton_Clicked(object sender, EventArgs e)
    {
        var title = $"[User Report] {this.exceptionName}";
        var body = this.ExceptionMessage;

        var url = IssueUrl
            .Replace(TitlePlaceholder, HttpUtility.UrlEncode(title))
            .Replace(BodyPlaceholder, HttpUtility.UrlEncode(body));

        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });

        this.Close();
    }

    public static void ShowException(Exception exception)
    {
        var exceptionDialog = new ExceptionDialog(exception);
        exceptionDialog.ShowDialog();
        return;
    }

    public static void ShowException(string exceptionName, string exceptionMessage)
    {
        var exceptionDialog = new ExceptionDialog(exceptionName, exceptionMessage);
        exceptionDialog.ShowDialog();
        return;
    }
}
