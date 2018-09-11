using System.Collections.Generic;
using Newtonsoft.Json;
using TankGame.Domain.GameItem;
using UnityEngine;
using UnityEngine.Networking;

namespace TankGame.Network.Messages
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

    public abstract class UnitMessageBase : GameMessageBase
    {
        public int UnitId;

        public UnitMessageBase()
        {
        }

        public UnitMessageBase(int unitId)
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

    public class UnitPositionMessage : UnitMessageBase
    {
        public override GameMsgType Type
        {
            get { return GameMsgType.UnitPosition; }
        }

        public Vector3 Position;
        public float RotationY;

        public UnitPositionMessage()
        {
        }

        public UnitPositionMessage(Vector3 position, float rotationY)
        {
            Position = position;
            RotationY = rotationY;
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(Position);
            writer.Write(RotationY);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Position = reader.ReadVector3();
            RotationY = reader.ReadSingle();
        }
    }

    public class UnitDestroyMessage : UnitMessageBase
    {
        public override GameMsgType Type
        {
            get { return GameMsgType.UnitDestroy; }
        }
    }

    public class BulletStartedMsg : UnitMessageBase
    {
        public override GameMsgType Type
        {
            get { return GameMsgType.BulletStarted; }
        }

        public Vector3 Target;
        public float Force;

        public BulletStartedMsg()
        {
        }

        public BulletStartedMsg(Vector3 target, float force)
        {
            Target = target;
            Force = force;
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(Target);
            writer.Write(Force);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Target = reader.ReadVector3();
            Force = reader.ReadSingle();
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
        {
        }

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
            get { return GameMsgType.ConnectToBattleRequest; }
        }
    }

    public class TankRespawnMsg : GameMessageBase
    {
        public override GameMsgType Type
        {
            get { return GameMsgType.OpponentRespawn; }
        }

        public GameBattlePlayerInfo Opponent;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(JsonConvert.SerializeObject(Opponent));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Opponent = JsonConvert.DeserializeObject<GameBattlePlayerInfo>(reader.ReadString());
        }
    }

    public class DropItemInfo
    {
        public GameItemType CatalogId;
        public Vector3 Pos;
        public int ItemSpawnPointIndex;
        public long WorldId;
    }

    public class ConnectToBattleAnswerMsg : GameMessageBase
    {
        public override GameMsgType Type
        {
            get { return GameMsgType.ConnectToBattleAnswer; }
        }

        public bool IsCanConnect;
        public GameBattlePlayerInfo Player;
        public List<GameBattlePlayerInfo> Opponents;
        public List<DropItemInfo> DroppedItems;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(IsCanConnect);
            writer.Write(JsonConvert.SerializeObject(Player));
            writer.Write(JsonConvert.SerializeObject(Opponents));
            writer.Write(JsonConvert.SerializeObject(DroppedItems));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            IsCanConnect = reader.ReadBoolean();
            Player = JsonConvert.DeserializeObject<GameBattlePlayerInfo>(reader.ReadString());
            Opponents = JsonConvert.DeserializeObject<List<GameBattlePlayerInfo>>(reader.ReadString());
            DroppedItems = JsonConvert.DeserializeObject<List<DropItemInfo>>(reader.ReadString());
        }
    }

    public class CreateDropItemMsg : GameMessageBase
    {
        public override GameMsgType Type
        {
            get { return GameMsgType.CreateDropItem; }
        }

        public DropItemInfo DropInfo;

        public CreateDropItemMsg()
        {
        }

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
            get { return GameMsgType.PickupGameItem; }
        }

        public DropItemInfo DropInfo;

        public PickUpGameItemMsg()
        {
        }

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