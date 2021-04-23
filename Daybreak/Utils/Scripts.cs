namespace Daybreak.Utils
{
    public static class Scripts
    {
        public const string SendSelectionOnContextMenu = @"
            document.addEventListener('contextmenu', function (event)
            {
                var text = '';
                var activeEl = document.activeElement;
                var activeElTagName = activeEl ? activeEl.tagName.toLowerCase() : null;
                if ((activeElTagName == 'textarea') || (activeElTagName == 'input' &&
                    /^(?:text|search|password|tel|url)$/i.test(activeEl.type)) &&
                    (typeof activeEl.selectionStart == 'number'))
                {
                    text = activeEl.value.slice(activeEl.selectionStart, activeEl.selectionEnd);
                }
                    else if (window.getSelection)
                {
                    text = window.getSelection().toString();
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
