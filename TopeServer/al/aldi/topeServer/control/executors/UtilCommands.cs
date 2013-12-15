using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        public static TopeResponse writeClipBoard(TopeRequest request)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = true;
            UtilsCommandExecutor.writeClipBoard(request.arg0);
            return topeResponse;
        }

        public static TopeResponse ping(TopeRequest request)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = true;
            return topeResponse;
        }

        public static TopeResponse quitTope(TopeRequest request)
        {
            TopeResponse topeResponse = new TopeResponse();
            var quitThread = new Thread(
                () =>
                {
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                });

            topeResponse.success = true;

            quitThread.Start();
            return topeResponse;
        }

        public static TopeResponse shortcuts(TopeRequest request)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = ProgCommandExecutor.appInputSimulation(request.arg0);
            return topeResponse;
        }

        public static TopeResponse uploadFile(TopeRequest request)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = request.nancyRequest.Files.Count() != 0;
            HttpFile file = request.nancyRequest.Files.FirstOrDefault();
            if (topeResponse.success)
            {
                try
                {
                    String destination = IOUtils.GetUserHomeFolder() + "\\" + file.Name;
                    if (File.Exists(destination) && !Boolean.Parse(request.arg0))
                    {
                        topeResponse.success = false;
                        topeResponse.message = file.Name + " Exists";
                    }
                    else
                    {
                        Stream output = File.OpenWrite(destination);
                        IOUtils.CopyStream(file.Value, output);
                        topeResponse.success = true;
                    }
                }
                catch (Exception e)
                {
                    TopeLogger.Log(e.Message);
                    topeResponse.success = false;
                }
            }
            return topeResponse;

        }
    }
}
