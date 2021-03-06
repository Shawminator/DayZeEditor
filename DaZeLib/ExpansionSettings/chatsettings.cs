using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class ChatSettings
    {
        const int CurrentVersion = 2;

        public int m_Version { get; set; }
        public int EnableGlobalChat { get; set; }
        public int EnablePartyChat { get; set; }
        public int EnableTransportChat { get; set; }
        public Chatcolors ChatColors { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ChatSettings()
        {
            m_Version = CurrentVersion;
            ChatColors = new Chatcolors();
            isDirty = true;
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
            }
        }
    }

    public class Chatcolors
    {
        public string SystemChatColor { get; set; }
        public string AdminChatColor { get; set; }
        public string GlobalChatColor { get; set; }
        public string DirectChatColor { get; set; }
        public string TransportChatColor { get; set; }
        public string PartyChatColor { get; set; }
        public string TransmitterChatColor { get; set; }

        public Chatcolors()
        {
            SystemChatColor = "BA45BAFF";
            AdminChatColor = "C0392BFF";
            GlobalChatColor = "58C3F7FF";
            DirectChatColor = "FFFFFFFF";
            TransportChatColor = "FFCE09FF";
            PartyChatColor = "FFCE09FF";
            TransmitterChatColor = "F9FF49FF";
        }
    }

}
