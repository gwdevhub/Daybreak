-- Per-project Neovim config for the Daybreak repo.
-- Auto-sourced when nvim is started inside the repo, provided the user has
-- `vim.o.exrc = true` in their config (or runs `:set exrc`). Neovim 0.9+
-- prompts to trust this file on first load.
--
-- Commands added:
--   :DaybreakStageX86 [Config]   build x86 components via wine and stage
--                                them into Daybreak.Linux/bin/<Config>/...
--                                /{Injector,Api}/ (default Config: Debug)
--   :DaybreakRunLinux            run Daybreak.Linux via `dotnet run` in a
--                                terminal split (honors launchSettings.json,
--                                which easy-dot-net's launcher does not)
--
-- Note: a Linux/wine attach-debugger workflow was attempted (winedbg --gdb
-- stub *and* native Linux gdb -p attach), but both freeze GW reliably because
-- of ptrace/wineserver interactions that wine doesn't tolerate. For now,
-- prefer logging-based debugging via the API's console + log file in the
-- game directory (Daybreak.API<YYYYMMDD>.log next to Gw.exe).

local repo = vim.fn.fnamemodify(debug.getinfo(1, 'S').source:sub(2), ':p:h')
local scripts = repo .. '/Scripts'

-- ───── Progress notification with spinner ──────────────────────────────────
-- Uses nvim-notify / snacks.nvim's `id`/`replace` semantics to update a
-- single notification in place. Falls back to plain notify if the backend
-- doesn't support replace (the spinner just stops updating).
local SPINNER = { '⣷', '⣯', '⣟', '⡿', '⢿', '⣻', '⣽', '⣾' }

local function start_progress(title, message)
    -- stable id so snacks.nvim/nvim-notify replace the same notification
    local id = 'daybreak-' .. title:gsub('%s+', '-') .. '-' .. tostring(vim.uv.hrtime())
    local state = { title = title, msg = message, frame = 1, done = false, id = id }
    local function render()
        local icon = state.done and '' or (SPINNER[state.frame] .. ' ')
        local opts = {
            title = state.title,
            id = state.id,
            timeout = state.done and 4000 or false,
            hide_from_history = not state.done,
            icon = state.done and '✓' or nil,
        }
        if state.notif then opts.replace = state.notif end
        state.notif = vim.notify(icon .. state.msg, vim.log.levels.INFO, opts)
    end
    render()
    state.timer = vim.uv.new_timer()
    state.timer:start(80, 80, vim.schedule_wrap(function()
        if state.done then return end
        state.frame = (state.frame % #SPINNER) + 1
        render()
    end))
    return {
        update = function(msg) state.msg = msg; render() end,
        finish = function(ok, msg)
            state.done = true
            if state.timer then state.timer:stop(); state.timer:close(); state.timer = nil end
            state.msg = msg or state.msg
            local final_opts = {
                title = state.title,
                id = state.id,
                timeout = 4000,
                icon = ok and '✓' or '✗',
            }
            if state.notif then final_opts.replace = state.notif end
            vim.notify((ok and '✓ ' or '✗ ') .. state.msg,
                ok and vim.log.levels.INFO or vim.log.levels.ERROR, final_opts)
        end,
    }
end

-- ───── :DaybreakStageX86 [Config] ─────────────────────────────────────────
vim.api.nvim_create_user_command('DaybreakStageX86', function(opts)
    local config = opts.args ~= '' and opts.args or 'Debug'
    local progress = start_progress('Daybreak StageX86', 'Building x86 components (' .. config .. ')…')
    vim.fn.jobstart({ scripts .. '/StageX86ForDebug.sh', config, '' }, {
        cwd = repo,
        on_exit = function(_, code)
            vim.schedule(function()
                if code == 0 then
                    progress.finish(true, 'Staged x86 (' .. config .. ')')
                else
                    progress.finish(false, 'Stage failed (exit ' .. code ..
                        '). See /tmp/wine-injector-publish.log, /tmp/wine-api-publish.log')
                end
            end)
        end,
        stdout_buffered = false,
        stderr_buffered = false,
    })
end, {
    nargs = '?',
    complete = function() return { 'Debug', 'Release' } end,
    desc = 'Build x86 components in wine and stage into Daybreak.Linux output',
})

-- ───── :DaybreakRunLinux ───────────────────────────────────────────────────
-- easy-dot-net launches the produced binary directly, bypassing launchSettings.json.
-- This runs Daybreak.Linux through `dotnet run`, which honors the environment
-- variables (GDK_BACKEND, WEBKIT_DISABLE_DMABUF_RENDERER, etc.) declared there.
-- Opens in the current window (no split) so it integrates with normal buffer
-- navigation; Ctrl-\ Ctrl-N to leave terminal mode, then your usual buffer
-- bindings work.
vim.api.nvim_create_user_command('DaybreakRunLinux', function()
    vim.cmd('enew')
    vim.fn.termopen({ 'dotnet', 'run', '--project', 'Daybreak.Linux/Daybreak.Linux.csproj', '-c', 'Debug' },
                    { cwd = repo })
    vim.cmd('startinsert')
end, { desc = 'Run Daybreak.Linux via dotnet run in the current window' })
