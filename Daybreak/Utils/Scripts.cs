namespace Daybreak.Utils;

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

    public const string GetHrefFromSkillPage = @"
            new function(){
                var img = document.getElementsByClassName('fullImageLink')[0].childNodes[0].childNodes[0];
                function getDataUrl(img) {
                    // Create canvas
                    const canvas = document.createElement('canvas');
                    const ctx = canvas.getContext('2d');
                    // Set width and height
                    canvas.width = img.width;
                    canvas.height = img.height;
                    // Draw the image
                    ctx.drawImage(img, 0, 0);
                    return canvas.toDataURL('image/jpeg');
                }
                
                console.log(img.src);
                var imageBase64 = getDataUrl(img);
                console.log(imageBase64);
                let jsonObject =
                {
                    skillUrl: document.URL,
                    skillImage: imageBase64
                }
                window.chrome.webview.postMessage(jsonObject);
                return jsonObject;
            }";
}
