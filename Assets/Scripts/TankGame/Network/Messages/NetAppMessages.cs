using System;
using System.Collections.Generic;
using DMarketSDK.Market;
using Newtonsoft.Json;
using TankGame.Domain.GameItem;
using TankGame.Domain.PlayerData;
using TankGame.Inventory.Domain;
using UnityEngine.Networking;

namespace TankGame.Network.Messages
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
        public NetworkMessageErrorType Error;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write((short)Error);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Error = (NetworkMessageErrorType)reader.ReadInt16();
        }

        public bool HasError { get { return Error != NetworkMessageErrorType.None; } }
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

        public string MarketToken; // TODO: i think this filed not used in any proccess, maybe need to remove.
        public DMarketDataLoadResponse Response;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(MarketToken);
            writer.Write(JsonConvert.SerializeObject(Response));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            MarketToken = reader.ReadString();
            Response = JsonConvert.DeserializeObject<DMarketDataLoadResponse>(reader.ReadString());
        }
    }

    public class AppChangingItemsMessage : AppMessageBase
    {
        [Serializable]
        public class RequestParams
        {
            public ItemActionType ActionType;
            public List<long> WorldIds;
            public List<GameItemType> ItemTypes;

            public int ItemCount
            {
                get { return WorldIds.Count; }
            }

            public RequestParams()
            {
                WorldIds = new List<long>();
                ItemTypes = new List<GameItemType>();
            }
        }

        public RequestParams Params;

        public override AppMsgType Type
        {
            get { return AppMsgType.ItemChangingRequest; }
        }

        public ItemActionType ActionType
        {
            get { return Params.ActionType; }
        }

        public int ItemsCount
        {
            get { return Params.ItemCount; }
        }

        public AppChangingItemsMessage() : this(ItemActionType.Unknow)
        {
        }

        public AppChangingItemsMessage(ItemActionType actionType)
        {
            Params = new RequestParams() {ActionType = actionType};
        }

        public void AddItem(long worldId, GameItemType itemType)
        {
            Params.ItemTypes.Add(itemType);
            Params.WorldIds.Add(worldId);
        }

        public long GetWorldId(int itemIndex)
        {
            return Params.WorldIds[itemIndex];
        }

        public GameItemType GetItemType(int itemIndex)
        {
            return Params.ItemTypes[itemIndex];
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(JsonConvert.SerializeObject(Params));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Params = JsonConvert.DeserializeObject<RequestParams>(reader.ReadString());
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

        public DMarketDataLoadResponse Response;

        public AppLoadDMarketAnswerMessage(DMarketDataLoadResponse response)
        {
            Response = response;
        }

        /// <summary>
        /// Default constructor for serialization.
        /// </summary>
        public AppLoadDMarketAnswerMessage() { }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(JsonConvert.SerializeObject(Response));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Response = JsonConvert.DeserializeObject<DMarketDataLoadResponse>(reader.ReadString());
        }
    }

    public class AppChangingItemsAnswerMessage : AppServerAnswerMessageBase
    {
        public override AppMsgType Type { get { return AppMsgType.ItemChangingAnswer; } }

        public ItemsChangingResponse Response;

        public AppChangingItemsAnswerMessage(ItemsChangingResponse response)
        {
            Response = response;
        }

        /// <summary>
        /// Default constructor for serialization.
        /// </summary>
        public AppChangingItemsAnswerMessage() { }

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

        public DMarketGameTokenResponse Response;

        public AppGetGameTokenAnswerMessage()
        {
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(JsonConvert.SerializeObject(Response));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Response = JsonConvert.DeserializeObject<DMarketGameTokenResponse>(reader.ReadString());
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

        public AppUpdateMarketTokenMessage(string marketToken, string refreshMarketToken)
        {
            MarketToken = marketToken;
            RefreshMarketToken = refreshMarketToken;
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

        public MarketMoveItemRequestParams TransactionData;

        public AppDMarketTransactionMessage() { }

        public AppDMarketTransactionMessage(MarketMoveItemRequestParams transactionData)
        {
            TransactionData = transactionData;
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(JsonConvert.SerializeObject(TransactionData));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            TransactionData = JsonConvert.DeserializeObject< MarketMoveItemRequestParams>(reader.ReadString());
        }
    }

    public class InventoryBasicIntegrationMessage : AppMessageBase
    {
        public override AppMsgType Type { get { return AppMsgType.LoadInventoryBasicIntegration; } }
    }

    public class InventoryBasicIntegrationAnswerMessage : AppServerAnswerMessageBase
    {
        public override AppMsgType Type { get { return AppMsgType.LoadInventoryBasicIntegrationAnswer; } }

        public DMarketDataLoadResponse Response { get; private set; }

        public InventoryBasicIntegrationAnswerMessage() { }

        public InventoryBasicIntegrationAnswerMessage(DMarketDataLoadResponse response)
        {
            Response = response;
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(JsonConvert.SerializeObject(Response));
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);
            Response = JsonConvert.DeserializeObject<DMarketDataLoadResponse>(reader.ReadString());
        }
    }

    public class UnloadInventoryBasicIntegrationMessage : AppMessageBase
    {
        public override AppMsgType Type { get { return AppMsgType.UnloadInventoryBasicIntegration; } }
    }
}