// Interop bridge between the Blazor FocusView and the vendored Gridstack.js library.
//
// Ownership model (kept deliberately simple to avoid the two engines fighting each other):
//  * Blazor owns the tile *markup* and renders the initial geometry as gs-x/gs-y/gs-w/gs-h.
//  * While editing, Gridstack owns drag/resize *entirely* and runs natively for the whole
//    session - there are no per-move callbacks into .NET, which is what kept the previous
//    implementation feeling janky.
//  * .NET pulls the final geometry on demand via save() (on Save / show / hide), never on
//    every mouse move.
window.focusGrid = (() => {
    let grid = null;
    let resizeObserver = null;
    let gridElement = null;
    let gridRows = 0;

    function computeCellHeight(element, rows) {
        const height = element.clientHeight;
        if (!height || !rows) {
            return 'auto';
        }

        return Math.max(1, Math.floor(height / rows));
    }

    function init(element, options) {
        if (!window.GridStack) {
            console.error('GridStack library is not loaded');
            return;
        }

        destroy();
        gridElement = element;
        gridRows = options.rows;

        grid = window.GridStack.init({
            column: options.columns,
            cellHeight: computeCellHeight(element, options.rows),
            margin: 2,
            // float:false gives "gravity" - tiles compact upwards into any free space above them.
            float: false,
            disableDrag: !options.editing,
            disableResize: !options.editing,
            staticGrid: !options.editing,
            draggable: { cancel: '.tile-remove-button' },
            resizable: { handles: 'se' },
            animate: true
        }, element);

        // Keep the cells filling the available height as the window/container resizes.
        resizeObserver = new ResizeObserver(() => {
            if (!grid || !gridElement) {
                return;
            }

            grid.cellHeight(computeCellHeight(gridElement, gridRows));
        });
        resizeObserver.observe(element);
    }

    // Returns the current geometry of every tile so .NET can persist it. 1-based coordinates
    // to match the FocusViewTile model.
    function save() {
        if (!grid) {
            return [];
        }

        return grid.getGridItems().map(el => {
            const node = el.gridstackNode || {};
            return {
                component: el.getAttribute('data-component'),
                column: (node.x || 0) + 1,
                row: (node.y || 0) + 1,
                columnSpan: node.w || 1,
                rowSpan: node.h || 1
            };
        });
    }

    function destroy() {
        if (resizeObserver) {
            resizeObserver.disconnect();
            resizeObserver = null;
        }

        if (grid) {
            grid.destroy(false);
            grid = null;
        }

        gridElement = null;
    }

    return { init, save, destroy };
})();
