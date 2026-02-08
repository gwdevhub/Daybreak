window.hoverDelay = (() => {
    let activeHover = null;
    let progressElement = null;
    let progressRing = null;
    let animationFrame = null;

    const HOVER_DURATION_MS = 1300;
    const HOVER_FILL_DURATION_MS = 1000;
    const CIRCLE_RADIUS = 12;
    const STROKE_WIDTH = 3;
    const SVG_SIZE = (CIRCLE_RADIUS + STROKE_WIDTH) * 2;
    const CIRCLE_CENTER = SVG_SIZE / 2;
    const CIRCUMFERENCE = 2 * Math.PI * CIRCLE_RADIUS;

    function createProgressElement() {
        if (progressElement) return;

        const container = document.getElementById('main-app-container');
        if (!container) {
            console.error('main-app-container not found');
            return;
        }

        progressElement = document.createElement('div');
        progressElement.style.cssText = `
            position: fixed;
            pointer-events: none;
            z-index: 9999;
            display: none;
            width: ${SVG_SIZE}px;
            height: ${SVG_SIZE}px;
        `;
        progressElement.innerHTML = `
            <svg width="${SVG_SIZE}" height="${SVG_SIZE}" style="transform: rotate(-90deg);">
                <circle cx="${CIRCLE_CENTER}" cy="${CIRCLE_CENTER}" r="${CIRCLE_RADIUS}" 
                    fill="none" 
                    stroke="var(--neutral-stroke-rest)"
                    stroke-width="${STROKE_WIDTH}"
                    opacity="0.3"/>
                <circle cx="${CIRCLE_CENTER}" cy="${CIRCLE_CENTER}" r="${CIRCLE_RADIUS}" 
                    fill="none" 
                    stroke="var(--accent-fill-hover)"
                    stroke-width="${STROKE_WIDTH}"
                    stroke-dasharray="${CIRCUMFERENCE}"
                    stroke-dashoffset="${CIRCUMFERENCE}"
                    class="progress-ring"/>
            </svg>
        `;
        container.appendChild(progressElement);
        progressRing = progressElement.querySelector('.progress-ring');
    }

    function updatePosition(x, y) {
        if (progressElement && activeHover) {
            progressElement.style.left = `${x + 10}px`;
            progressElement.style.top = `${y + 25}px`;
        }
    }

    function start(dotNetRef, callbackMethod) {
        createProgressElement();
        console.debug('Hover start requested');
        if (activeHover) {
            stop();
        }

        const startTime = Date.now();
        if (progressElement) {
            progressElement.style.display = 'block';
        }

        const animate = () => {
            const elapsed = Date.now() - startTime;
            const progress = Math.min(elapsed / HOVER_FILL_DURATION_MS, 1);

            if (progressRing) {
                const offset = CIRCUMFERENCE * (1 - progress);
                progressRing.style.strokeDashoffset = offset;
            }

            if (elapsed >= HOVER_DURATION_MS) {
                stop();
                dotNetRef.invokeMethodAsync(callbackMethod);
            } else {
                animationFrame = requestAnimationFrame(animate);
            }
        };

        activeHover = { dotNetRef, callbackMethod };
        animationFrame = requestAnimationFrame(animate);
    }

    function stop() {
        activeHover = null;
        console.debug('Hover stopped');
        if (animationFrame) {
            cancelAnimationFrame(animationFrame);
            animationFrame = null;
        }
        if (progressElement) {
            progressElement.style.display = 'none';
            if (progressRing) {
                progressRing.style.strokeDashoffset = `${CIRCUMFERENCE}`;
            }
        }
    }

    document.addEventListener('mousemove', (e) => {
        updatePosition(e.clientX, e.clientY);
    });

    return {
        start,
        stop
    };
})();