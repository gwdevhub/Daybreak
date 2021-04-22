namespace Daybreak.Utils
{
    public static class Scripts
    {
        public const string AlterContextMenu = @"
            document.addEventListener('contextmenu', function (event)
            {
                var text = '';
                if (window.getSelection)
                {
                    text = window.getSelection().toString();
                } 
                else if (document.selection && document.selection.type != 'Control')
                {
                    text = document.selection.createRange().text;
                }

                let jsonObject =
                {
                    Key: 'ContextMenu',
                    Value:
                    {
                        X: event.screenX,
                        Y: event.screenY,
                        Selection: text
                    }
                };
                window.chrome.webview.postMessage(jsonObject);
            });";
    }
}
