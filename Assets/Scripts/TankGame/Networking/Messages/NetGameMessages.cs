using Newtonsoft.Json;
using PlayerData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking.Msg
{
    public abstract class GameMessageBase : MessageBase
    {
        public abstract GameMsgType Type { get; }

        public int ClientId;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(ClientId);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            ClientId = reader.ReadInt32();
        }
    }

    public abstract class GameUnitMessageBase : GameMessageBase
    {
        public int UnitId;

        public GameUnitMessageBase()
        {
        }

        public GameUnitMessageBase(int unitId)
        {
            UnitId = unitId;
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(UnitId);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            UnitId = reader.ReadInt32();
        }
    }

    public class UnitMovedMsg : GameUnitMessageBase
    {
        public override GameMsgType Type
        {
            get
            {
                return GameMsgType.UnitMoved;
            }
        }
        
        public Vector3 Pos;
        public float RotY;

        public UnitMovedMsg()
        { }

        public UnitMovedMsg(Vector3 pos, float rotY)
        {
            Pos = pos;
            RotY = rotY;
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(Pos);
            writer.Write((double)RotY);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Pos = reader.ReadVector3();
            RotY = (float)reader.ReadDouble();
        }
    }

    public class TankDiedMsg : GameUnitMessageBase
    {
        public override GameMsgType Type
        {
            get
            {
                return GameMsgType.Died;
            }
        }
    }

    public class BulletStartedMsg : GameUnitMessageBase
    {
        public override GameMsgType Type
        {
            get
            {
                return GameMsgType.BulletStarted;
            }
        }

        public Vector3 Target;
        public float Force;

        public BulletStartedMsg()
        { }

        public BulletStartedMsg(Vector3 target, float force)
        {
            Target = target;
            Force = force;
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(Target);
            writer.Write((double)Force);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Target = reader.ReadVector3();
            Force = (float)reader.ReadDouble();
        }
    }

    public class GameBattlePlayerInfo
    {
        public int UnitId;
        public string UserName;
        public Vector3 Position;
        public List<GameItemType> EquippedItemsTypes;
        public bool IsAlive = true;

        public GameBattlePlayerInfo()
        { }

        public GameBattlePlayerInfo(int unitId, string userName, Vector3 position, List<GameItemType> equippedItems)
        {
            UnitId = unitId;
            UserName = userName;
            Position = position;
            EquippedItemsTypes = equippedItems;
        }
    }

    public class ConnectToBattleRequestMsg : GameMessageBase
    {
        public override GameMsgType Type
        {
            get
            {
                return GameMsgType.ConnectToBattleRequest;
            }
        }
        

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
        }
    }

    public class TankRespawnMsg : GameMessageBase
    {
        public override GameMsgType Type
        {
            get
            {
                return GameMsgType.OponentRespawn;
            }
        }

        public GameBattlePlayerInfo Oponent;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(JsonConvert.SerializeObject(Oponent));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Oponent = JsonConvert.DeserializeObject<GameBattlePlayerInfo>(reader.ReadString());
        }
    }

    public class TankStateUpdateMsg : GameUnitMessageBase
    {
        public override GameMsgType Type
        {
            get
            {
                return GameMsgType.TankStateUpdate;
            }
        }

        public Vector3 Pos;

        public TankStateUpdateMsg()
        {
        }

        public TankStateUpdateMsg(Vector3 pos)
        {
            Pos = pos;
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(Pos);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Pos = reader.ReadVector3();
        }
    }

    public class DropItemInfo
    {
        public GameItemType CatalogId;
        public Vector3 Pos;
        public long WorldId;
    }

    public class ConnectToBattleAnswerMsg : GameMessageBase
    {
        public override GameMsgType Type
        {
            get
            {
                return GameMsgType.ConnectToBattleAnswer;
            }
        }

        public bool IsCanConnect;
        public GameBattlePlayerInfo Player;
        public List<GameBattlePlayerInfo> Oponents;
        public List<DropItemInfo> DropedItems;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(IsCanConnect);
            writer.Write(JsonConvert.SerializeObject(Player));
            writer.Write(JsonConvert.SerializeObject(Oponents));
            writer.Write(JsonConvert.SerializeObject(DropedItems));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            IsCanConnect = reader.ReadBoolean();
            Player = JsonConvert.DeserializeObject<GameBattlePlayerInfo>(reader.ReadString());
            Oponents = JsonConvert.DeserializeObject<List<GameBattlePlayerInfo>>(reader.ReadString());
            DropedItems = JsonConvert.DeserializeObject<List<DropItemInfo>>(reader.ReadString());
        }
    }

    public class CreateDropItemMsg : GameMessageBase
    {
        public override GameMsgType Type
        {
            get
            {
                return GameMsgType.CreateDropItem;
            }
        }

        public CreateDropItemMsg()
        { }

        public DropItemInfo DropInfo;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(JsonConvert.SerializeObject(DropInfo));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            DropInfo = JsonConvert.DeserializeObject<DropItemInfo>(reader.ReadString());
        }
    }

    public class PickUpGameItemMsg : GameMessageBase
    {
        public override GameMsgType Type
        {
            get
            {
                return GameMsgType.PickupGameItem;
            }
        }

        public DropItemInfo DropInfo;

        public PickUpGameItemMsg()
        { }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(JsonConvert.SerializeObject(DropInfo));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            DropInfo = JsonConvert.DeserializeObject<DropItemInfo>(reader.ReadString());
        }
    }
}