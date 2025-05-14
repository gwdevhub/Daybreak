using Daybreak.Shared.Services.Mutex;
using Daybreak.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Daybreak.Services.Mutex;

internal sealed class MutexHandler : IMutexHandler
{
    public void CloseMutex(Process process, string mutexName)
    {
        CloseHandle(process, mutexName);
    }

    private static List<NativeMethods.SystemHandleInformation> GetHandles(Process targetProcess, IntPtr systemHandle)
    {
        var processHandles = new List<NativeMethods.SystemHandleInformation>();
        var basePointer = systemHandle.ToInt64();
        NativeMethods.SystemHandleInformation currentHandleInfo;
        for (int i = 0; i < Marshal.ReadInt32(systemHandle); i++)
        {
            var currentOffset = IntPtr.Size + (i * Marshal.SizeOf(typeof(NativeMethods.SystemHandleInformation)));
            currentHandleInfo = (NativeMethods.SystemHandleInformation)Marshal.PtrToStructure(new IntPtr(basePointer + currentOffset), typeof(NativeMethods.SystemHandleInformation))!;
            if (currentHandleInfo.OwnerPID == (uint)targetProcess.Id)
            {
                processHandles.Add(currentHandleInfo);
            }
        }

        return processHandles;
    }

    private static void CloseHandle(Process targetProcess, string handleName)
    {
        var systemHandles = GetAllHandles();
        if (systemHandles == IntPtr.Zero)
        {
            return;
        }

        List<NativeMethods.SystemHandleInformation> processHandles = GetHandles(targetProcess, systemHandles);
        Marshal.FreeHGlobal(systemHandles);
        var processHandle = NativeMethods.OpenProcess(NativeMethods.ProcessAccessFlags.DupHandle, false, (uint)targetProcess.Id);
        foreach (var handleInfo in processHandles)
        {
            if (GetHandleName(handleInfo, processHandle).Contains(handleName))
            {
                if (CloseOwnedHandle(handleInfo.OwnerPID, new IntPtr(handleInfo.HandleValue)))
                {
                    NativeMethods.CloseHandle(processHandle);
                    return;
                }
            }
        }

        NativeMethods.CloseHandle(processHandle);
        return;
    }

    private static string GetHandleName(NativeMethods.SystemHandleInformation targetHandleInfo, IntPtr processHandle)
    {
        if (targetHandleInfo.AccessMask.ToInt64() == 0x0012019F)
        {
            return string.Empty;
        }

        try
        {
            var thisProcess = Process.GetCurrentProcess().Handle;
            NativeMethods.DuplicateHandle(processHandle, new IntPtr(targetHandleInfo.HandleValue), thisProcess, out var handle, 0, false, NativeMethods.DuplicateOptions.DUPLICATE_SAME_ACCESS);
            var bufferSize = GetHandleNameLength(handle);
            var stringBuffer = Marshal.AllocHGlobal(bufferSize);
            NativeMethods.NtQueryObject(handle, NativeMethods.ObjectInformationClass.ObjectNameInformation, stringBuffer, bufferSize, out _);
            NativeMethods.CloseHandle(handle);
            var handleName = ConvertToString(stringBuffer);
            Marshal.FreeHGlobal(stringBuffer);
            return handleName;
        }
        catch(Exception e)
        {
            return string.Empty;
        }
    }

    private static IntPtr GetAllHandles()
    {
        int bufferSize = 0x10000;
        var pSysInfoBuffer = Marshal.AllocHGlobal(bufferSize);
        var queryResult = NativeMethods.NtQuerySystemInformation(NativeMethods.SystemInformationClass.SystemHandleInformation,
            pSysInfoBuffer, bufferSize, out _);

        while (queryResult == NativeMethods.NtStatus.STATUS_INFO_LENGTH_MISMATCH)
        {
            Marshal.FreeHGlobal(pSysInfoBuffer);
            bufferSize *= 2;
            pSysInfoBuffer = Marshal.AllocHGlobal(bufferSize);
            queryResult = NativeMethods.NtQuerySystemInformation(NativeMethods.SystemInformationClass.SystemHandleInformation,
                pSysInfoBuffer, bufferSize, out _);
        }

        if (queryResult == NativeMethods.NtStatus.STATUS_SUCCESS)
        {
            return pSysInfoBuffer;
        }
        else
        {
            Marshal.FreeHGlobal(pSysInfoBuffer);
            return IntPtr.Zero;
        }
    }

    private static int GetHandleNameLength(IntPtr handle)
    {
        var infoBufferSize = Marshal.SizeOf(typeof(NativeMethods.ObjectBasicInformation));
        var pInfoBuffer = Marshal.AllocHGlobal(infoBufferSize);
        NativeMethods.NtQueryObject(handle, NativeMethods.ObjectInformationClass.ObjectBasicInformation, pInfoBuffer, infoBufferSize, out _);
        NativeMethods.ObjectBasicInformation objInfo = (NativeMethods.ObjectBasicInformation)Marshal.PtrToStructure(pInfoBuffer, typeof(NativeMethods.ObjectBasicInformation))!;
        Marshal.FreeHGlobal(pInfoBuffer);
        if (objInfo.NameInformationLength == 0)
        {
            return 0x100;
        }
        else
        {
            return (int)objInfo.NameInformationLength;
        }
    }

    private static string ConvertToString(IntPtr stringBuffer)
    {
        var baseAddress = stringBuffer.ToInt64();
        var offset = IntPtr.Size * 2;
        var handleName = Marshal.PtrToStringUni(new IntPtr(baseAddress + offset));
        return handleName!;
    }

    private static bool CloseOwnedHandle(uint processId, IntPtr handleToClose)
    {
        var processHandle = NativeMethods.OpenProcess(NativeMethods.ProcessAccessFlags.All, false, processId);
        var success = NativeMethods.DuplicateHandle(processHandle, handleToClose, IntPtr.Zero, out _, 0, false, NativeMethods.DuplicateOptions.DUPLICATE_CLOSE_SOURCE);
        NativeMethods.CloseHandle(processHandle);
        return success;
    }
}
