using System;
using Akka.Actor;
using Game.ActorModel.Actors;
using Game.ActorModel.ExternalSystems;

namespace Game.Web.Models
{
    public static class GameActorSystem
    {
        private static ActorSystem _actorSystem;
        private static IGameEventsPusher _gameEventsPusher;

        public static void Create()
        {
            _gameEventsPusher = new SignalRgamEventPusher();

            _actorSystem = ActorSystem.Create("GameSystem");

            // this get's the actor ref from the remoter actor system.
            ActorReferences.GameController = _actorSystem
                .ActorSelection("akka.tcp://GameSystem@127.0.0.1:8091/user/GameController")
                .ResolveOne(TimeSpan.FromSeconds(3))
                .Result;

            ActorReferences.SignalRBridge = _actorSystem.ActorOf(
                Props.Create(() => new SignalRBridgeActor(_gameEventsPusher, ActorReferences.GameController)), 
                "SignalRBridge"
            );

        }

        public static void Shutdown()
        {
            _actorSystem.Shutdown();
            _actorSystem.AwaitTermination(TimeSpan.FromSeconds(1));
        }

        public static class ActorReferences
        {
            public static IActorRef GameController { get; set; }
            public static IActorRef SignalRBridge { get; set; }
        }

    }
}