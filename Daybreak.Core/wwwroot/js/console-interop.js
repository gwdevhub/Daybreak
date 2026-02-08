(function() {
    'use strict';
    
    // Store original console methods
    const originalConsole = {
        log: console.log,
        info: console.info,
        warn: console.warn,
        error: console.error,
        debug: console.debug,
        trace: console.trace
    };
    
    let dotNetReference = null;
    
    function createConsoleWrapper(level, originalMethod) {
        return function(...args) {
            // Call original console method first
            originalMethod.apply(console, args);
            
            // Send to backend if available
            if (dotNetReference) {
                try {
                    const message = args.map(arg => {
                        if (typeof arg === 'object') {
                            try
                            {
                                return JSON.stringify(arg, null, 2);
                            }
                            catch
                            {
                                return String(arg);
                            }
                        }

                        return String(arg);
                    }).join(' ');
                    
                    let stack = null;
                    if (level === 'error' || level === 'warn') {
                        try
                        {
                            stack = new Error().stack;
                        }
                        catch
                        {
                        }
                    }
                    
                    dotNetReference.invokeMethodAsync('LogFromJavaScript', level, message, stack);
                }
                catch (error)
                {
                    originalConsole.error('Console interop failed:', error);
                }
            }
        };
    }
    
    // Override console methods
    console.log = createConsoleWrapper('log', originalConsole.log);
    console.info = createConsoleWrapper('info', originalConsole.info);
    console.warn = createConsoleWrapper('warn', originalConsole.warn);
    console.error = createConsoleWrapper('error', originalConsole.error);
    console.debug = createConsoleWrapper('debug', originalConsole.debug);
    console.trace = createConsoleWrapper('trace', originalConsole.trace);
    
    // Global initialization function
    window.blazorConsoleInterop = {
        initialize: function(dotNetRef) {
            dotNetReference = dotNetRef;
            console.info('Console redirection initialized');
        },
        
        dispose: function() {
            if (dotNetReference) {
                dotNetReference.dispose();
                dotNetReference = null;
            }
        }
    };
    
    console.info('Console interop script loaded');
})();