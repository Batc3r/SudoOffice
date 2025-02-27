﻿/* GPOffice (ver. Alpha 0.0.1) */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using UnityEngine;
using GPOffice.Modes;
using MEC;
using Exiled.API.Features.Items;

namespace GPOffice
{
    public class GPOffice : Plugin<Config>
    {
        public static GPOffice Instance;

        public List<string> Owner = new List<string>() { "76561199691978741@steam" };
        public bool AutoNuke = false;
        public Dictionary<string, float> OnGround = new Dictionary<string, float>();

        public static Dictionary<object, object> Mods = new Dictionary<object, object>()
            {
                {"로켓 런처", "FF8000/무슨 이유로든 피격당하면 승천합니다!/RocketLauncher"}, {"무제한", "3F13AB/무제한을 악용하지 않는 것을 추천합니다./Unlimited"}, {"슈퍼 스타", "FE2EF7/모두의 마이크가 공유됩니다!/SuperStar"},
                {"뒤통수 얼얼", "DF0101/아군 공격이 허용됩니다!/FriendlyFire"}, {"스피드왜건", "FFBF00/모두의 속도가 최대값으로 올라가는 대신에\n최대 체력이 반으로 줄어듭니다!/SpeedWagon"},
                {"랜덤박스", "BFFF00/60초마다 랜덤한 아이템을 얻을 수 있습니다!/RandomItem"}, {"스피드런", "FF0000/가장 먼저 탈출구에 도달한 죄수가 승리합니다!/SpeedRun"},
                {"평화로운 재단", "00FF00/시설 내에는 SCP만 없을 뿐입니다../NoSCP"},  /*{"상습범", "610B21/모두의 손에 제일버드가 쥐어집니다./Jailbird"},*/
                {"트리플업", "F4FA58/모드 3개가 합쳐집니다!/TripleUp"}, {"스피릿", "CED8F6/죽으면 영혼 상태에 돌입합니다!/Spirit"},{"워크스테이션 업그레이드", "00FFFF/워크스테이션에서 업그레이드하세요!/ABattle"}, {"나 홀로 집에", "FA5882/SCP가 점령한 재단 속 한명의 죄수만 남았습니다./OnlyOneHuman"},
                {"표적", "F7BE81/현상금 수배자를 죽이면 승리합니다!/BountyHunter"}, {"밀집", "04B45F/모두가 한 곳에 스폰됩니다./Dense"},
                {"스즈메의 문단속", "00FFFF/문 너머 다른 차원./DoorLock"}
        };
        public Dictionary<string, List<Vector3>> Maps = new Dictionary<string, List<Vector3>>()
            {
                {"dust", new List<Vector3>() { new Vector3(53.26172f, 1040.629f, -31.19531f), new Vector3(23.61328f, 1037.999f, -43.21484f), new Vector3(22.21875f, 1037.996f, -58.96875f),
                                               new Vector3(15.63281f, 1037.996f, -58.85547f), new Vector3(2.316406f, 1039.301f, -60.15234f), new Vector3(1.617188f, 1039.141f, -86.67188f),
                                               new Vector3(13.26172f, 1040.617f, -108.5156f), new Vector3(29.76563f, 1039.965f, -102.4492f), new Vector3(28f, 1037.996f, -83.48438f), new Vector3(51.28906f, 1037.996f, -56.47656f),
                                               new Vector3(31.28125f, 1035.387f, -97.19922f), new Vector3(47.57813f, 1037.999f, -42.97266f), new Vector3(57.39844f, 1035.696f, -75.83594f), new Vector3(72.95975f, 1038.644f, -76.43791f),
                                               new Vector3(68.43631f, 1040.629f, -43.37151f), new Vector3(67.52615f, 1038.644f, -61.91447f), new Vector3(71.90897f, 1037.996f, -82.12541f), new Vector3(75.31912f, 1038.668f, -104.3949f),
                                               new Vector3(58.19333f, 1037.013f, -91.25609f)}},
                {"pl", new List<Vector3>() { new Vector3(73.09416f, 906.5809f, -420.5794f), new Vector3(84.474f, 906.5807f, -421.0674f), new Vector3(79.13398f, 901.457f, -450.8738f),
                                             new Vector3(55.44602f, 901.4601f, -468.0944f), new Vector3(68.73533f, 901.457f, -473.2781f), new Vector3(94.03555f, 901.457f, -473.6239f),
                                             new Vector3(94.20408f, 901.457f, -500.6101f), new Vector3(64.25095f, 901.457f, -501.7078f), new Vector3(60.31037f, 901.457f, -500.6549f),
                                             new Vector3(99.79726f, 901.457f, -467.5458f), new Vector3(112.787f, 901.457f, -460.8142f), new Vector3(62.97807f, 901.457f, -451.8979f),
                                             new Vector3(95.4621f, 902.2148f, -452.7215f)} },
                {"ru", new List<Vector3>() { new Vector3(-2.371094f, 1003.158f, 36.24609f), new Vector3(-7.160156f, 1003.158f, 36.24609f), new Vector3(-7.546875f, 1003.158f, 31.12109f),
                                             new Vector3(-7.005299f, 1003.158f, 27.98828f), new Vector3(-2.363281f, 1003.158f, 31.08814f), new Vector3(4.617188f, 1003.158f, 27.06641f),
                                             new Vector3(4.667969f, 1003.158f, 35.59375f), new Vector3(-0.5195313f, 1003.158f, 36.27344f), new Vector3(-2.15625f, 1003.158f, 28.17188f)} },
                {"wh", new List<Vector3>() { new Vector3(48.96993f, 977.7834f, -73.72765f), new Vector3(37.21875f, 977.7835f, -88.23047f), new Vector3(42.61328f, 978.1492f, -83.76953f),
                                             new Vector3(24.28516f, 977.7845f, -93.21094f), new Vector3(34.28125f, 977.7834f, -87.48047f), new Vector3(18.08304f, 977.7834f, -73.70633f),
                                             new Vector3(-4.113281f, 977.7835f, -69.28125f), new Vector3(47.94922f, 977.7834f, -69.15234f),new Vector3(11.375f, 980.7399f, -92.83203f),
                                             new Vector3(-0.9101563f, 977.7834f, -92.57813f), new Vector3(16.17963f, 977.7834f, -81.57955f), new Vector3(14.59375f, 977.7834f, -73.18359f),
                                             new Vector3(-5.0625f, 977.7834f, -72.53906f)} }
            };
        public Dictionary<object, object> Players = new Dictionary<object, object>();

        public static object Mode = GetRandomValue(Mods.Keys.ToList());
        public string mod = Mode.ToString();

        public static T GetRandomValue<T>(List<T> list)
        {
            System.Random random = new System.Random();
            int index = random.Next(0, list.Count);
            return list[index];
        }

        public override void OnEnabled()
        {
            Instance = this;

            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Player.Left += OnLeft;

            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;

            Exiled.Events.Handlers.Warhead.Stopping += OnStopping;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Player.Left -= OnLeft;

            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;

            Exiled.Events.Handlers.Warhead.Stopping -= OnStopping;

            Instance = null;
        }

        public async void OnRoundStarted()
        {
            // 선택된 모드의 설명을 모두에게 띄워줍니다.
            Player.List.ToList().ForEach(x => x.Broadcast(10, $"<size=30>⌈<color=#{Mods[mod].ToString().Split('/')[0]}><b>{mod}</b></color>⌋</size>\n<size=25>{Mods[mod].ToString().Split('/')[1]}</size>"));
            ServerConsole.AddLog($"다음 모드가 선택되었습니다. [{mod}]", color: ConsoleColor.Blue);

            var modeType = Type.GetType($"GPOffice.Modes.{Mods[mod].ToString().Split('/')[2].Replace(" ", "")}");
            if (modeType != null)
            {
                var modeInstance = Activator.CreateInstance(modeType);
                var onEnabledMethod = modeType.GetMethod("OnEnabled");
                onEnabledMethod?.Invoke(modeInstance, null);
            }

            // OnSpawned or OnChangingRole 이벤트 핸들
            foreach (var player in Player.List)
            {
                if (player.Role.Type == PlayerRoles.RoleTypeId.Scp106)
                {
                    player.Role.Set(PlayerRoles.RoleTypeId.Tutorial);
                    await Task.Delay(10);
                    player.Role.Set(PlayerRoles.RoleTypeId.Scp106);
                }
                else
                    player.Role.Set(player.Role);
            }

            Timing.RunCoroutine(IsFallDown());
            Timing.CallDelayed(15 * 60f, async () =>
            {
                if (Warhead.IsDetonated)
                {
                    AutoNuke = true;
                    Server.ExecuteCommand("/cassie_sl 시간이 너무 오래 걸립니다! 모두의 체력이 초당 1%씩 줄어듭니다!");

                    while (true)
                    {
                        Player.List.ToList().ForEach(x => x.Health -= (x.MaxHealth / 100));
                        await Task.Delay(1000);
                    }
                }
                else
                {
                    AutoNuke = true;
                    Warhead.Start();
                    Server.ExecuteCommand("/cassie_sl <color=red>예정된 시설 자폭 프로세스가 시작되었습니다.</color> <b>대피하십시오.</b>");
                }
            });
        }
        public async void OnRoundEnded(Exiled.Events.EventArgs.Server.RoundEndedEventArgs ev)
        {
            Server.FriendlyFire = true;

            await Task.Delay(9000);
            Server.ExecuteCommand($"sr");
        }

        public async void OnVerified(Exiled.Events.EventArgs.Player.VerifiedEventArgs ev)
        {
            OnGround.Add(ev.Player.UserId, 5);

            if (Round.IsStarted)
                ev.Player.Broadcast(10, $"<size=30>⌈<color=#{Mods[mod].ToString().Split('/')[0]}><b>{mod}</b></color>⌋</size>\n<size=25>{Mods[mod].ToString().Split('/')[1]}</size>");

            else
            {
                string modes = string.Join(", ", Mods.Keys).Trim();
                int colorIndex = 0;

                while (!Round.IsStarted)
                {
                    string[] modeList = modes.Split(',');
                    StringBuilder coloredModes = new StringBuilder();

                    for (int i = 0; i < modeList.Length; i++)
                    {
                        if (i % modeList.Length == colorIndex)
                        {
                            coloredModes.Append($"<color=yellow>{modeList[i]}</color>");
                        }
                        else
                        {
                            coloredModes.Append(modeList[i]);
                        }

                        if (i != modeList.Length - 1)
                        {
                            coloredModes.Append(", ");
                        }
                    }

                    ev.Player.ShowHint($"<align=left><b>——————————————</b>\n<i>Welcome, {ev.Player.DisplayNickname}!</i>\nGP: 0\n<u>Score: 0</u>\n<b>——————————————</b></align>\n\n\n\n<align=left><b>아래 모드들 중 하나의 모드가 선택됩니다.</b>\n<size=20>{coloredModes}</size></align>\n\n\n\n\n", 3);
                    await Task.Delay(500);
                    colorIndex = (colorIndex + 1) % modeList.Length;
                }

                ev.Player.ShowHint($"", 1);
            }
        }

        public void OnLeft(Exiled.Events.EventArgs.Player.LeftEventArgs ev)
        {
            if (OnGround.ContainsKey(ev.Player.UserId))
                OnGround.Remove(ev.Player.UserId);
        }

        public void OnStopping(Exiled.Events.EventArgs.Warhead.StoppingEventArgs ev)
        {
            if (AutoNuke)
                ev.IsAllowed = false;
        }

        public IEnumerator<float> IsFallDown()
        {
            while (true)
            {
                foreach (var player in Player.List)
                {
                    if (player.IsAlive && OnGround.ContainsKey(player.UserId) && !player.IsNoclipPermitted && player.Role.Type != PlayerRoles.RoleTypeId.Scp079)
                    {
                        if (Physics.Raycast(player.Position, Vector3.down, out RaycastHit hit, 10, (LayerMask)1))
                            OnGround[player.UserId] = 5;
                        else
                        {
                            OnGround[player.UserId] -= 0.1f;

                            if (OnGround[player.UserId] <= 0)
                                player.Kill("공허에 빨려들어갔습니다.");
                        }
                    }
                }

                yield return Timing.WaitForSeconds(0.1f);
            }
        }
    }
}

