using ETModel;

namespace ETHotfix
{
	[Event(EventIdType.InitSceneStart)]
	public class InitSceneStart_CreateLoginBallsUI : AEvent
	{
		public override void Run()
		{
			UI ui = Game.Scene.GetComponent<UIComponent>().Create(UIType.UILoginBalls);
		}
	}
}
