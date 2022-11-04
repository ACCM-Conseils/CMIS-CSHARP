using System;

namespace CmisObjectModel.EventBus
{

    [Flags()]
    public enum enumBuiltInEvents : int
    {
        BeginCancelCheckout = CancelCheckout | enumBuiltInEventMasks.flgBegin,
        BeginCheckIn = CheckIn | enumBuiltInEventMasks.flgBegin,
        BeginDeleteObject = DeleteObject | enumBuiltInEventMasks.flgBegin,

        CancelCheckout = 4,
        CheckIn = 8,
        DeleteObject = 16,

        EndCancelCheckout = CancelCheckout | enumBuiltInEventMasks.flgEnd,
        EndCheckIn = CheckIn | enumBuiltInEventMasks.flgEnd,
        EndDeleteObject = DeleteObject | enumBuiltInEventMasks.flgEnd
    }

    [Flags()]
    public enum enumBuiltInEventMasks : int
    {
        flgBegin = 1,
        flgEnd = 2,

        maskEventNames = enumBuiltInEvents.CancelCheckout | enumBuiltInEvents.CheckIn | enumBuiltInEvents.DeleteObject,
        maskBeginOrEnd = flgBegin | flgEnd
    }

    public enum enumEventBusListenerResult : int
    {
        success = 0,
        removeListener = 1,
        dontCare = 2
    }
}