window.wikiRenderer = {
    render: async function (filePath, targetId) {
        const element = document.getElementById(targetId);
        if (!element) {
            console.error('wiki-renderer: Element not found:', targetId);
            return;
        }

        if (typeof marked === 'undefined') {
            console.error('wiki-renderer: marked.js not loaded');
            element.innerHTML = '<p>Markdown renderer not loaded.</p>';
            return;
        }

        try {
            const response = await fetch(filePath);
            if (!response.ok) {
                element.innerHTML = '<p>Page not found.</p>';
                return;
            }

            const markdown = await response.text();
            let html = marked.parse(markdown);
            
            // Rewrite relative wiki links to local wiki paths
            html = this.rewriteWikiLinks(html);

            // Rewrite relative image paths to absolute wiki paths
            html = this.rewriteImagePaths(html);
            
            element.innerHTML = html;

            // Add IDs to headings for anchor navigation
            this.addHeadingIds(element);

            // Add click handler for anchor links to prevent Blazor interception
            this.setupAnchorLinks(element);
        } catch (error) {
            console.error('wiki-renderer: Failed to load markdown:', error);
            element.innerHTML = '<p>Failed to load page.</p>';
        }
    },

    addHeadingIds: function (container) {
        const headings = container.querySelectorAll('h1, h2, h3, h4');
        headings.forEach(heading => {
            if (!heading.id) {
                // Generate ID from heading text (same logic as typical markdown parsers)
                const id = heading.textContent
                    .toLowerCase()
                    .trim()
                    .replace(/[^\w\s-]/g, '')
                    .replace(/\s+/g, '-');
                heading.id = id;
            }
        });
    },

    setupAnchorLinks: function (container) {
        const anchorLinks = container.querySelectorAll('a[href^="#"]');
        anchorLinks.forEach(link => {
            link.addEventListener('click', (e) => {
                e.preventDefault();
                e.stopPropagation();
                const targetId = link.getAttribute('href').substring(1);
                const targetElement = document.getElementById(targetId);
                if (targetElement) {
                    targetElement.scrollIntoView({ behavior: 'smooth' });
                }
            });
        });
    },

    rewriteWikiLinks: function (html) {
        // Handle relative wiki links (e.g., href="PageName" or href="PageName.md")
        // Convert to /wiki/PageName (stripping .md extension if present)
        const relativeWikiPattern = /href="(?!http|\/|#)([^"]+)"/g;
        html = html.replace(relativeWikiPattern, (match, pageName) => {
            // Remove .md extension if present
            const cleanName = pageName.replace(/\.md$/i, '');
            return `href="/wiki/${cleanName}"`;
        });

        return html;
    },

    rewriteImagePaths: function (html) {
        // Rewrite relative image src paths to absolute wiki paths
        // src="assets/image.png" -> src="/wiki/assets/image.png"
        const relativeImagePattern = /src="(?!http|\/|data:)([^"]+)"/g;
        html = html.replace(relativeImagePattern, 'src="/wiki/$1"');

        return html;
    }
};