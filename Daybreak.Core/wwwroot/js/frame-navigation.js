// Hosts wiki pages prepared by WikiPageLoader and relays their navigation requests to the
// owning Blazor component. Only messages from a registered frame's own window are forwarded.
window.frameNavigation = (() => {
    const frames = new Map();

    window.addEventListener('message', event => {
        const data = event.data;
        const owner = frames.get([...frames.keys()].find(frame => frame.contentWindow === event.source))?.owner;
        if (!owner || !data) {
            return;
        }

        if (data.type === 'daybreak-frame-navigate' && typeof data.url === 'string') {
            owner.invokeMethodAsync('OnFrameNavigate', data.url);
        } else if (data.type === 'daybreak-frame-history' && (data.delta === -1 || data.delta === 1)) {
            owner.invokeMethodAsync('OnFrameHistory', data.delta);
        }
    });

    // location.replace() keeps the frame out of the browser's history. Pushed entries would
    // be restored as blank pages by the webview's own back/forward handling.
    function load(frame, url) {
        const state = frames.get(frame);
        if (state?.blobUrl) {
            URL.revokeObjectURL(state.blobUrl);
        }

        if (state) {
            state.blobUrl = url.startsWith('blob:') ? url : null;
        }

        frame.contentWindow?.location.replace(url);
    }

    return {
        attach: (frame, owner) => frames.set(frame, { owner, blobUrl: null }),
        detach: frame => {
            const state = frames.get(frame);
            if (state?.blobUrl) {
                URL.revokeObjectURL(state.blobUrl);
            }

            frames.delete(frame);
        },
        show: (frame, html) => load(frame, URL.createObjectURL(new Blob([html], { type: 'text/html' }))),
        clear: frame => load(frame, 'about:blank'),
    };
})();
