using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.ActionHandlers
{
    public class LookAroundHandler : BaseHandler, ILookAroundHandler
    {
        public LookAroundHandler(ILogger<LookAroundHandler> log,
            IResponseBuilder responseBuilder,
            IMessageProvider messageProvider,
            ILocationProvider locationProvider,
            IMapHelper mapHelper,
            AdventureContext context)
            : base(log, responseBuilder, messageProvider, locationProvider, mapHelper, context)
        { }
            
        public override void Handle()
        {
            _log.LogInformation("Handling LookAround");

            // Can't see in the dark.
            if (!_advContext.IsCurrentLocationLight())
            {
                _responseBuilder.PrefixResponse(MsgMnemonic.MovePitchDark, 1);
                return;
            }

            var curLoc = _advContext.CurrentLocation;
            var lookStr = _advContext.GetParameterValue("visuals");
            var directionStr = _advContext.GetParameterValue("directions");
            if (directionStr != null)
            {
                _responseBuilder.AddToResponse(MsgMnemonic.CantGiveMoreDetail, 1);
                _responseBuilder.AddToResponse(curLoc.Description);
            }
            else
            {
                _responseBuilder.AddToResponse(curLoc.Description);
                _mapHelper.EnumerateItemsHere();
            }
        }
    }
}
