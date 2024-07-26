using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{

    public class ExpansionChatSettings
    {
        const int CurrentVersion = 4;

        public int m_Version { get; set; }
        public int EnableGlobalChat { get; set; }
        public int EnablePartyChat { get; set; }
        public int EnableTransportChat { get; set; }
        public ExpansionChatColors ChatColors { get; set; }
        public BindingList<string> BlacklistedWords { get;set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionChatSettings()
        {
            m_Version = CurrentVersion;
            EnableGlobalChat = 1;
            EnablePartyChat = 1;
            EnableTransportChat = 1;
            ChatColors = new ExpansionChatColors();
            BlacklistedWords = new BindingList<string>();
        }

        public bool checkver()
        {
            if (m_Version != CurrentVersion)
            {
                m_Version = CurrentVersion;
                isDirty = true;
                return true;
            }
            return false;
        }

        public string getcolourfromcontrol(string name)
        {
            switch (name)
            {
                case "SystemChatColorPB":
                    return ChatColors.SystemChatColor;
                case "AdminChatColorPB":
                    return ChatColors.AdminChatColor;
                case "GlobalChatColorPB":
                    return ChatColors.GlobalChatColor;
                case "DirectChatColorPB":
                    return ChatColors.DirectChatColor;
                case "TransportChatColorPB":
                    return ChatColors.TransportChatColor;
                case "PartyChatColorPB":
                    return ChatColors.PartyChatColor;
                case "TransmitterChatColorPB":
                    return ChatColors.TransmitterChatColor;
                case "StatusMessageColorPB":
                    return ChatColors.StatusMessageColor;
                case "ActionMessageColorPB":
                    return ChatColors.ActionMessageColor;
                case "FriendlyMessageColorPB":
                    return ChatColors.FriendlyMessageColor;
                case "ImportantMessageColorPB":
                    return ChatColors.ImportantMessageColor;
                case "DefaultMessageColorPB":
                    return ChatColors.DefaultMessageColor;
            }
            return "";
        }
        public void setcolour(string name, string Colour)
        {
            Colour = Colour.Substring(2) + Colour.Substring(0, 2);
            switch (name)
            {
                case "SystemChatColorPB":
                    ChatColors.SystemChatColor = Colour;
                    break;
                case "AdminChatColorPB":
                    ChatColors.AdminChatColor = Colour;
                    break;
                case "GlobalChatColorPB":
                    ChatColors.GlobalChatColor = Colour;
                    break;
                case "DirectChatColorPB":
                    ChatColors.DirectChatColor = Colour;
                    break;
                case "TransportChatColorPB":
                    ChatColors.TransportChatColor = Colour;
                    break;
                case "PartyChatColorPB":
                    ChatColors.PartyChatColor = Colour;
                    break;
                case "TransmitterChatColorPB":
                    ChatColors.TransmitterChatColor = Colour;
                    break;
                case "StatusMessageColorPB":
                    ChatColors.StatusMessageColor = Colour;
                    break;
                case "ActionMessageColorPB":
                    ChatColors.ActionMessageColor = Colour;
                    break;
                case "FriendlyMessageColorPB":
                    ChatColors.FriendlyMessageColor = Colour;
                    break;
                case "ImportantMessageColorPB":
                    ChatColors.ImportantMessageColor = Colour;
                    break;
                case "DefaultMessageColorPB":
                    ChatColors.DefaultMessageColor = Colour;
                    break;
            }
        }
    }

    public class ExpansionChatColors
    {
        public string SystemChatColor { get; set; }
        public string AdminChatColor { get; set; }
        public string GlobalChatColor { get; set; }
        public string DirectChatColor { get; set; }
        public string TransportChatColor { get; set; }
        public string PartyChatColor { get; set; }
        public string TransmitterChatColor { get; set; }
        public string StatusMessageColor { get; set; }
        public string ActionMessageColor { get; set; }
        public string FriendlyMessageColor { get; set; }
        public string ImportantMessageColor { get; set; }
        public string DefaultMessageColor { get; set; }


        public ExpansionChatColors()
        {
            SystemChatColor = "BA45BAFF";
            AdminChatColor = "C0392BFF";
            GlobalChatColor = "58C3F7FF";
            DirectChatColor = "FFFFFFFF";
            TransportChatColor = "FFCE09FF";
            PartyChatColor = "FFCE09FF";
            TransmitterChatColor = "F9FF49FF";
            StatusMessageColor = "4B77BEFF";
            ActionMessageColor = "F7CA18FF";
            FriendlyMessageColor = "2ECC71FF";
            ImportantMessageColor = "F22613FF";
            DefaultMessageColor = "FFFFFFFF";
        }
    }

}
