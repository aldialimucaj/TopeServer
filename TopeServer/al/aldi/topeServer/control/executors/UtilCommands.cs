using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopeServer.al.aldi.topeServer.model;
using TopeServer.al.aldi.topeServer.model.responses;
using TopeServer.al.aldi.utils.general;

namespace TopeServer.al.aldi.topeServer.control.executors
{
    public class UtilCommands
    {
        public static TopeResponse showMsg(TopeRequest request)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = UtilsCommandExecutor.showMsg(request.arg0);
            return topeResponse;
        }

        public static TopeResponse beep(TopeRequest request)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = UtilsCommandExecutor.beep(request.arg0);
            return topeResponse;
        }

        public static TopeResponse readOutLoud(TopeRequest request)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = UtilsCommandExecutor.readOutLoud(request.arg0);
            return topeResponse;
        }
        public static TopeResponse readClipBoard(TopeRequest request)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = true;
            SimpleTextResponse.ClassWithStringMessage payload = new SimpleTextResponse.ClassWithStringMessage();
            payload.message = UtilsCommandExecutor.readClipBoard();
            topeResponse.setPayload(payload);
            return topeResponse;
        }
    }
}
