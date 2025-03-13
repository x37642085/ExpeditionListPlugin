using Grabacr07.KanColleWrapper;
using Livet;
using Livet.EventListeners;
using System.Collections.Generic;
using MetroTrilithon.Mvvm;
using System;
using System.Reactive.Linq;
using System.Linq;
using System.Windows;
using Grabacr07.KanColleViewer.Composition;
using Grabacr07.KanColleWrapper.Models;

namespace ExpeditionListPlugin
{
    public class ExpeditionViewModel : ViewModel
    {
        #region isAllArea
        private Boolean _isAllArea;
        public Boolean isAllArea
        {
            get
            {
                return _isAllArea;
            }
            set
            {
                _isAllArea = value;
                isArea1Contain = value;
                isArea1ExContain = value;
                isArea2Contain = value;
                isArea2ExContain = value;
                isArea3Contain = value;
                isArea4Contain = value;
                isArea4ExContain = value;
                isArea5Contain = value;
                isArea5ExContain = value;
                isArea7Contain = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region isArea1Contain 変更通知プロパティ
        private Boolean _isArea1Contain;
        public Boolean isArea1Contain
        {
            get
            {
                return _isArea1Contain;
            }
            set
            {
                _isArea1Contain = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region isArea1ExContain 変更通知プロパティ
        private Boolean _isArea1ExContain;
        public Boolean isArea1ExContain
        {
            get
            {
                return _isArea1ExContain;
            }
            set
            {
                _isArea1ExContain = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region isArea2Contain 変更通知プロパティ
        private Boolean _isArea2Contain;
        public Boolean isArea2Contain
        {
            get
            {
                return _isArea2Contain;
            }
            set
            {
                _isArea2Contain = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region isArea2ExContain 変更通知プロパティ
        private Boolean _isArea2ExContain;
        public Boolean isArea2ExContain
        {
            get
            {
                return _isArea2ExContain;
            }
            set
            {
                _isArea2ExContain = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region isArea3Contain 変更通知プロパティ
        private Boolean _isArea3Contain;
        public Boolean isArea3Contain
        {
            get
            {
                return _isArea3Contain;
            }
            set
            {
                _isArea3Contain = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region isArea4Contain 変更通知プロパティ
        private Boolean _isArea4Contain;
        public Boolean isArea4Contain
        {
            get
            {
                return _isArea4Contain;
            }
            set
            {
                _isArea4Contain = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region isArea4ExContain 変更通知プロパティ
        private Boolean _isArea4ExContain;
        public Boolean isArea4ExContain
        {
            get
            {
                return _isArea4ExContain;
            }
            set
            {
                _isArea4ExContain = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region isArea5Contain 変更通知プロパティ
        private Boolean _isArea5Contain;
        public Boolean isArea5Contain
        {
            get
            {
                return _isArea5Contain;
            }
            set
            {
                _isArea5Contain = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region isArea5ExContain 変更通知プロパティ
        private Boolean _isArea5ExContain;
        public Boolean isArea5ExContain
        {
            get
            {
                return _isArea5ExContain;
            }
            set
            {
                _isArea5ExContain = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region isArea7Contain 変更通知プロパティ
        private Boolean _isArea7Contain;
        public Boolean isArea7Contain
        {
            get
            {
                return _isArea7Contain;
            }
            set
            {
                _isArea7Contain = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region ExpeditionInfos 変更通知プロパティ
        private List<ExpeditionInfo> _ExpeditionInfos;

        public List<ExpeditionInfo> ExpeditionInfos
        {
            get
            {
                return _ExpeditionInfos;
            }
            set
            {
                this._ExpeditionInfos = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        private readonly ExpeditionNotifier notifier;

        public ExpeditionViewModel(ExpeditionListPlugin plugin)
        {
            //KanColleClient.Current.Homeport.Organization
            //    .Subscribe(nameof(Organization.Fleets), this.InitializeFleets).AddTo(this);
            this.notifier = new ExpeditionNotifier(plugin);

            InitializeExpedition();
            KanColleProxy proxy = KanColleClient.Current.Proxy;
            proxy.api_port.Subscribe(x => this.UpdateView());
            proxy.api_get_member_ship3.Subscribe(x => this.UpdateView());
            proxy.api_req_hensei_change.Subscribe(x => this.UpdateView());
            proxy.ApiSessionSource
                .Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_hensei/preset_select").Subscribe(x => this.UpdateView());
        }

        private void InitializeExpedition()
        {
            isAllArea = true;
            UpdateExpedition();
            
            this.CompositeDisposable.Add(new PropertyChangedEventListener(KanColleClient.Current.Homeport.Organization)
            {
                {
                    () => KanColleClient.Current.Homeport.Organization.Ships,
                    (_, __) => { DispatcherHelper.UIDispatcher.Invoke(this.UpdateView); }
                }
            });
            this.CompositeDisposable.Add(new PropertyChangedEventListener(this)
            {
                {
                    () => this.isArea1Contain,
                    (_, __) => { DispatcherHelper.UIDispatcher.Invoke(this.UpdateView); }
                },
                {
                    () => this.isArea1ExContain,
                    (_, __) => { DispatcherHelper.UIDispatcher.Invoke(this.UpdateView); }
                },
                {
                    () => this.isArea2Contain,
                    (_, __) => { DispatcherHelper.UIDispatcher.Invoke(this.UpdateView); }
                },
                {
                    () => this.isArea2ExContain,
                    (_, __) => { DispatcherHelper.UIDispatcher.Invoke(this.UpdateView); }
                },
                {
                    () => this.isArea3Contain,
                    (_, __) => { DispatcherHelper.UIDispatcher.Invoke(this.UpdateView); }
                },
                {
                    () => this.isArea4Contain,
                    (_, __) => { DispatcherHelper.UIDispatcher.Invoke(this.UpdateView); }
                },
                {
                    () => this.isArea4ExContain,
                    (_, __) => { DispatcherHelper.UIDispatcher.Invoke(this.UpdateView); }
                },
                {
                    () => this.isArea5Contain,
                    (_, __) => { DispatcherHelper.UIDispatcher.Invoke(this.UpdateView); }
                },
                {
                    () => this.isArea5ExContain,
                    (_, __) => { DispatcherHelper.UIDispatcher.Invoke(this.UpdateView); }
                },
                {
                    () => this.isArea7Contain,
                    (_, __) => { DispatcherHelper.UIDispatcher.Invoke(this.UpdateView); }
                }
            });
        }

        public void SetArea(String area)
        {
            isAllArea = false;
            isArea1Contain = "鎮守".Equals(area) ? true : false;
            isArea1ExContain = "鎮守Ex".Equals(area) ? true : false;
            isArea2Contain = "南西諸島".Equals(area) ? true : false;
            isArea2ExContain = "南西諸島Ex".Equals(area) ? true : false;
            isArea3Contain = "北方".Equals(area) ? true : false;
            isArea7Contain = "南西海域".Equals(area) ? true : false;
            isArea4Contain = "西方".Equals(area) ? true : false;
            isArea4ExContain = "西方Ex".Equals(area) ? true : false;
            isArea5Contain = "南方".Equals(area) ? true : false;
            isArea5ExContain = "南方Ex".Equals(area) ? true : false;
        }

        private void UpdateExpedition()
        {
            var list = new List<ExpeditionInfo>();

            ExpeditionInfo.ExpeditionList.ToList().ForEach(info =>
            {
                if ((isArea1Contain && "鎮守".Equals(info.Area)) ||
                    (isArea1ExContain && "鎮守Ex".Equals(info.Area)) ||
                    (isArea2Contain && "南西諸島".Equals(info.Area)) ||
                    (isArea2ExContain && "南西諸島Ex".Equals(info.Area)) ||
                    (isArea3Contain && "北方".Equals(info.Area)) ||
                    (isArea7Contain && "南西海域".Equals(info.Area)) ||
                    (isArea4Contain && "西方".Equals(info.Area)) ||
                    (isArea4ExContain && "西方Ex".Equals(info.Area)) ||
                    (isArea5Contain && "南方".Equals(info.Area)) ||
                    (isArea5ExContain && "南方Ex".Equals(info.Area))
                    )
                {

                    info.Check();
                    list.Add(info);
                }
            });

            ExpeditionInfos = list;
        }
        private void UpdateView()
        {
            if (!KanColleClient.Current.IsStarted) return;

            UpdateExpedition();
        }
    }
}
