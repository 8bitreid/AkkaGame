using System;
using Akka.Actor;
using Game.ActorModel.Actors;

namespace Game.Web.Models
{
    public static class GameActorSystem
    {
        private static ActorSystem _actorSystem;

        public static void Create()
        {
            _actorSystem = ActorSystem.Create("GameSystem");
            ActorReferences.GameController = _actorSystem.ActorOf<GameControllerActor>();

        }

        public static void Shutdown()
        {
            _actorSystem.Shutdown();
            _actorSystem.AwaitTermination(TimeSpan.FromSeconds(1));
        }

        public static class ActorReferences
        {
            public static IActorRef GameController { get; set; }

        }

    }
}