using PlayerData;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Shop.Domain;

namespace Networking.Msg
{
    public abstract class AppMessageBase : MessageBase
    {
        public abstract AppMsgType Type { get; }
        public int ConnectionId;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(ConnectionId);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            ConnectionId = reader.ReadInt32();
        }
    }

    public class LoginMessage : AppMessageBase
    {
        public override AppMsgType Type
        {
            get
            {
                return AppMsgType.Login;
            }
        }

        public string UserName;
        public string Password;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(UserName);
            writer.Write(Password);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            UserName = reader.ReadString();
            Password = reader.ReadString();
        }
    }

    public class RegistrationMessage : AppMessageBase
    {
        public override AppMsgType Type
        {
            get
            {
                return AppMsgType.Registration;
            }
        }

        public string UserName;
        public string Password;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(UserName);
            writer.Write(Password);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            UserName = reader.ReadString();
            Password = reader.ReadString();
        }
    }

    public class LogoutMessage : AppMessageBase
    {
        public override AppMsgType Type
        {
            get
            {
                return AppMsgType.Logout;
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

    public abstract class AppServerAnswerMessageBase : AppMessageBase
    {
        public NetMsgErrorType Error;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write((short)Error);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Error = (NetMsgErrorType)reader.ReadInt16();
        }

        public bool HasError { get { return Error != NetMsgErrorType.None; } }
    }

    public class RegistrationAnswerMessage : AppServerAnswerMessageBase
    {
        public override AppMsgType Type
        {
            get
            {
                return AppMsgType.RegistrationAnswer;
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

    public class LoginAnswerMessage : AppServerAnswerMessageBase
    {
        public override AppMsgType Type
        {
            get
            {
                return AppMsgType.LoginAnswer;
            }
        }

        public PlayerInfo Data;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(JsonConvert.SerializeObject(Data));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Data = JsonConvert.DeserializeObject<PlayerInfo>(reader.ReadString());
        }
    }

    public class AppUpdatePlayerDataAnswerMessage : AppServerAnswerMessageBase
    {
        public override AppMsgType Type
        {
            get
            {
                return AppMsgType.UpdatePlayerDataAnswer;
            }
        }

        public PlayerInfo Data;

        public AppUpdatePlayerDataAnswerMessage() { }

        public AppUpdatePlayerDataAnswerMessage(PlayerInfo data)
        {
            Data = data;
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(JsonConvert.SerializeObject(Data));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Data = JsonConvert.DeserializeObject<PlayerInfo>(reader.ReadString());
        }
    }
    
    public class AppLoadDMarketDataMessage : AppMessageBase
    {
        public override AppMsgType Type
        {
            get
            {
                return AppMsgType.LoadDMarketDataRequest;
            }
        }

        public string MarketToken;
        public DMarketDataLoadResponce Responce;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(MarketToken);
            writer.Write(JsonConvert.SerializeObject(Responce));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            MarketToken = reader.ReadString();
            Responce = JsonConvert.DeserializeObject<DMarketDataLoadResponce>(reader.ReadString());
        }
    }

    public class AppChangingItemsMessage : AppMessageBase
    {
        public override AppMsgType Type
        {
            get
            {
                return AppMsgType.ItemChangingRequest;
            }
        }
        
        public ItemActionType ActionType;
        public long WorldId;
        public GameItemType ItemType;

        public AppChangingItemsMessage()
        {
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)ActionType);
            writer.Write(WorldId);
            writer.Write((int)ItemType);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            ActionType = (ItemActionType)reader.ReadInt32();
            WorldId = reader.ReadInt64();
            ItemType = (GameItemType)reader.ReadInt32();
        }
    }

    public class AppLoadDMarketAnswerMessage : AppServerAnswerMessageBase
    {
        public override AppMsgType Type
        {
            get
            {
                return AppMsgType.LoadDMarketDataAnswer;
            }
        }

        public DMarketDataLoadResponce Responce = new DMarketDataLoadResponce();

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(JsonConvert.SerializeObject(Responce));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Responce = JsonConvert.DeserializeObject<DMarketDataLoadResponce>(reader.ReadString());
        }
    }

    public class AppChangingItemsAnswerMessage : AppServerAnswerMessageBase
    {
        public override AppMsgType Type
        {
            get
            {
                return AppMsgType.ItemChangingAnswer;
            }
        }

        public ItemsChangingResponse Response;
        
        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(JsonConvert.SerializeObject(Response));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Response = JsonConvert.DeserializeObject<ItemsChangingResponse>(reader.ReadString());
        }
    }

    public class AppGetGameTokenMessage : AppMessageBase
    {
        public override AppMsgType Type
        {
            get
            {
                return AppMsgType.GetGameTokenRequest;
            }
        }

        public string UserId;

        public AppGetGameTokenMessage()
        {
        }

        public AppGetGameTokenMessage(string userId)
        {
            UserId = userId;
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(UserId);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            UserId = reader.ReadString();
        }
    }

    public class AppGetGameTokenAnswerMessage : AppServerAnswerMessageBase
    {
        public override AppMsgType Type
        {
            get
            {
                return AppMsgType.GetGameTokenAnswer;
            }
        }

        public DMarketGameTokenResponce Responce;

        public AppGetGameTokenAnswerMessage()
        {
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(JsonConvert.SerializeObject(Responce));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Responce = JsonConvert.DeserializeObject<DMarketGameTokenResponce>(reader.ReadString());
        }
    }

    public class AppUpdateMarketTokenMessage : AppMessageBase
    {
        public override AppMsgType Type
        {
            get
            {
                return AppMsgType.UpdateMarketToken;
            }
        }

        public string MarketToken;
        public string RefreshMarketToken;

        public AppUpdateMarketTokenMessage()
        {
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(MarketToken);
            writer.Write(RefreshMarketToken);

        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            MarketToken = reader.ReadString();
            RefreshMarketToken = reader.ReadString();
        }
    }

    public class AppDMarketTransactionMessage : AppMessageBase
    {
        public override AppMsgType Type
        {
            get
            {
                return AppMsgType.DMarketTransactionCompleted;
            }
        }
    }
}