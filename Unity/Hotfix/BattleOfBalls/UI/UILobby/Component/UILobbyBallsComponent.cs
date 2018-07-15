using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using ETModel;
using uTools;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UiLobbyBallsComponentSystem : AwakeSystem<UILobbyBallsComponent>
    {
        public override void Awake(UILobbyBallsComponent self)
        {
            self.Awake();
        }
    }
    [ObjectSystem]
    public class UiLobbyComponentSystemStart : StartSystem<UILobbyBallsComponent>
    {
        public override void Start(UILobbyBallsComponent self)
        {
            self.Start();
        }
    }

    public class UILobbyBallsComponent : Component
    {

        public static UILobbyBallsComponent Instance;
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        CancellationToken cancelToken;
        private TimerComponent timer;
        private Dictionary<int, Texture> DicTexture = new Dictionary<int, Texture>();

        /*private LongPressButton btnLong;
        private int longID = 0;*/

        #region 定义UI组件
        public Transform lobbyRoot;
        
        #endregion
        public void Awake()
        {
            Instance = this;
            this.lobbyRoot = this.GetParent<UI>().GameObject.transform;
        }

        public void Start()
        {
            this.timer = ETModel.Game.Scene.GetComponent<TimerComponent>();
            this.cancelToken = (CancellationToken)this.tokenSource.Token;
            this.InitUI();
            //this.ChangeTexture();
            this.InitEvent();
            //this.InitData();
            this.LoadGameAsync();
        }

        private async void LoadGameAsync()
        {
            await this.timer.WaitAsync(200, this.cancelToken);
        }

        public void CancelToken()
        {
            this.tokenSource.Cancel();
        }

        public void ResetToken()
        {
            this.tokenSource = new CancellationTokenSource();
            this.cancelToken = (CancellationToken)this.tokenSource.Token;
            this.UpdatePlayerInfo();

        }

        /// <summary>
        /// 初始化UI
        /// </summary>
        private void InitUI()
        {
        }

        public void UpdatePlayerInfo()
        {
            
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        private async void InitEvent()
        {
            
        }

        public void InitData()
        {
            ReferenceCollector rcTexture = this.lobbyRoot.Find("StaticResources/Textures").GetComponent<ReferenceCollector>();
            for (int i = 0; i < rcTexture.data.Count; i++)
            {
                this.DicTexture.Add(i, rcTexture.Get<Texture>(rcTexture.data[i].gameObject.name));
            }
        }
        
        public void HideLobby()
        {
            //this.MoveUI();
            this.lobbyRoot.gameObject.SetActive(false);
            this.CancelToken();
        }
        
        
        public async void BackLobby()
        {
            this.lobbyRoot.gameObject.SetActive(true);

        }
        public void LoadLobby()
        {
            this.lobbyRoot.gameObject.SetActive(true);
            this.ResetToken();
        }

    }
}
