using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.Hive.Plugin;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;

namespace MyFirstPlugin
{
    public class Accounts
    {
        public string username { get; set; }
        public string password { get; set; }
        public int user_id { get; set; }
        public string email { get; set; }
    }

    public class BooleanFactor
    {
        public int number { get; set; }
    }

    public class MyFirstPlugin : PluginBase
    {


        Database db = new Database();
        public MyFirstPlugin()
        {
            db.Connect("localhost", 3305, "serverassignment", "root", "Waterbottle2022");
        }

        static void Main(string[] args)
        {
        }
        public override string Name => "MyFirstPlugin";

        private IPluginLogger pluginLogger;


        public override bool SetupInstance(IPluginHost host, Dictionary<string, string> config, out string errorMsg)
        {
            this.pluginLogger = host.CreateLogger(this.Name);
            return base.SetupInstance(host, config, out errorMsg);
        }

        public override void OnCreateGame(ICreateGameCallInfo info)
        {
            this.pluginLogger.InfoFormat("OnCreateGame {0} by user {1}", info.Request.GameId, info.UserId);
            info.Continue(); // same as base.OnCreateGame(info);
        }

        public override void OnRaiseEvent(IRaiseEventCallInfo info)
        {
            base.OnRaiseEvent(info);

            if (info.Request.EvCode == 1)
            {
                string request = Encoding.Default.GetString((byte[])info.Request.Data);
                string response = "Message Received: " + request;
                this.PluginHost.BroadcastEvent(
                target: ReciverGroup.All,
                senderActor: 0,
                targetGroup: 0,
                data: new Dictionary<byte, object>() { { (byte)245, response } },
                evCode: info.Request.EvCode,
                cacheOp: 0
                );
            }
            else if (info.Request.EvCode == 2)
            {
                DataTable dt = db.Query("SELECT * FROM accounts");
                List<Accounts> accounts = Database.DataTableToList<Accounts>(dt);
                string response = string.Format("{0}",
                JsonConvert.SerializeObject(accounts));
                this.PluginHost.BroadcastEvent(
                recieverActors: new List<int>() { info.ActorNr },
                senderActor: 0,
                data: new Dictionary<byte, object>() { { (byte)245, response } },
                evCode: info.Request.EvCode,
                cacheOp: 0
                );
            }
            else if (info.Request.EvCode == 3)
            {
                db.Update("INSERT INTO accounts (username, password,email) VALUES ( " + Encoding.Default.GetString((byte[])info.Request.Data) + " );");
                DataTable dt = db.Query("SELECT * FROM accounts");
                List<Accounts> accounts = Database.DataTableToList<Accounts>(dt);
                string response = string.Format("{0}",
                JsonConvert.SerializeObject(accounts));
                this.PluginHost.BroadcastEvent(
                recieverActors: new List<int>() { info.ActorNr },
                senderActor: 0,
                data: new Dictionary<byte, object>() { { (byte)245, response } },
                evCode: info.Request.EvCode,
                cacheOp: 0
                );
            }
            else if (info.Request.EvCode == 4)
            {
                // Check username
                // Make it return 1 or 0
                // SELECT CASE WHEN EXISTS (SELECT username FROM accounts WHERE username = 'ellysou' ) THEN 1 ELSE 0 END;
                db.Update(" SELECT * FROM accounts WHERE username = '" + Encoding.Default.GetString((byte[])info.Request.Data) + "' ;");
                DataTable dt = db.Query(" SELECT * FROM accounts WHERE username = '" + Encoding.Default.GetString((byte[])info.Request.Data) + "' ;");
                List<BooleanFactor> bf = Database.DataTableToList<BooleanFactor>(dt);
                string response = string.Format("{0}",
                JsonConvert.SerializeObject(bf));
                this.PluginHost.BroadcastEvent(
                recieverActors: new List<int>() { info.ActorNr },
                senderActor: 0,
                data: new Dictionary<byte, object>() { { (byte)245, response } },
                evCode: info.Request.EvCode,
                cacheOp: 0
                );
            }
            else if (info.Request.EvCode == 5)
            {
                // Check password
                db.Update(" SELECT * FROM accounts WHERE email = '" + Encoding.Default.GetString((byte[])info.Request.Data) + "' ;");
                DataTable dt = db.Query(" SELECT * FROM accounts WHERE email = '" + Encoding.Default.GetString((byte[])info.Request.Data) + "' ;");
                List<BooleanFactor> bf = Database.DataTableToList<BooleanFactor>(dt);
                string response = string.Format("{0}",
                JsonConvert.SerializeObject(bf));
                this.PluginHost.BroadcastEvent(
                recieverActors: new List<int>() { info.ActorNr },
                senderActor: 0,
                data: new Dictionary<byte, object>() { { (byte)245, response } },
                evCode: info.Request.EvCode,
                cacheOp: 0
                );
            }
            else if (info.Request.EvCode == 6)
            {
                // Check email
                db.Update(" SELECT * FROM accounts WHERE " + Encoding.Default.GetString((byte[])info.Request.Data) + ";");
                DataTable dt = db.Query(" SELECT * FROM accounts WHERE " + Encoding.Default.GetString((byte[])info.Request.Data) + ";");
                List<BooleanFactor> bf = Database.DataTableToList<BooleanFactor>(dt);
                string response = string.Format("{0}",
                JsonConvert.SerializeObject(bf));
                this.PluginHost.BroadcastEvent(
                recieverActors: new List<int>() { info.ActorNr },
                senderActor: 0,
                data: new Dictionary<byte, object>() { { (byte)245, response } },
                evCode: info.Request.EvCode,
                cacheOp: 0
                );
            }
        }
    }
}
