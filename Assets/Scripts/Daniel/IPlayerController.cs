namespace Assets
{
    public interface IPlayerController
    {
		void Init(Player player);
		void Init(SinglePlayer player);
        void OnUpdate();
		void OnFixedUpdate();

    }
}
