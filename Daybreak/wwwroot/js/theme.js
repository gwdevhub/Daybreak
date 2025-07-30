window.themeInterop = {
    // Initialize theme system
    initialize: function() {
        this.updateTheme();
        
        // Listen for system theme changes
        if (window.matchMedia) {
            const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
            mediaQuery.addListener(() => {
                this.updateTheme();
            });
        }
    },

    // Update theme based on system preference
    updateTheme: function() {
        const isDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
        document.documentElement.setAttribute('data-theme', isDark ? 'dark' : 'light');
    },

    // Set theme colors from C# (when WPF theme changes)
    setThemeColors: function(backgroundColor, foregroundColor, accentColor) {
        const root = document.documentElement;
        
        if (backgroundColor) {
            root.style.setProperty('--background-color', backgroundColor);
            root.style.setProperty('--background-panel', this.adjustOpacity(backgroundColor, 0.8));
            root.style.setProperty('--background-modal', this.adjustOpacity(backgroundColor, 0.9));
            root.style.setProperty('--background-overlay', this.adjustOpacity(backgroundColor, 0.1));
        }
        
        if (foregroundColor) {
            root.style.setProperty('--foreground-color', foregroundColor);
            root.style.setProperty('--foreground-secondary', this.adjustOpacity(foregroundColor, 0.7));
            root.style.setProperty('--foreground-disabled', this.adjustOpacity(foregroundColor, 0.4));
        }
        
        if (accentColor) {
            root.style.setProperty('--accent-color', accentColor);
            root.style.setProperty('--accent-hover', this.adjustOpacity(accentColor, 0.8));
            root.style.setProperty('--accent-light', this.adjustOpacity(accentColor, 0.1));
            root.style.setProperty('--border-accent', this.adjustOpacity(accentColor, 0.3));
        }
    },

    // Helper function to adjust color opacity
    adjustOpacity: function(color, opacity) {
        // Simple implementation - you might want to use a more robust color library
        if (color.startsWith('#')) {
            return color + Math.floor(opacity * 255).toString(16).padStart(2, '0');
        } else if (color.startsWith('rgb(')) {
            return color.replace('rgb(', 'rgba(').replace(')', `, ${opacity})`);
        }
        return color;
    }
};

// Initialize when DOM is loaded
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => window.themeInterop.initialize());
} else {
    window.themeInterop.initialize();
}