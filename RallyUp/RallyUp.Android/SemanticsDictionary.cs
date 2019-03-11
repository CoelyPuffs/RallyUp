using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RallyUp
{
    class SemanticsDictionary
    {
        string invitation = "";

        List<string> actionStrings = new List<string>();
        List<string> positiveSemantics = new List<string>();
        List<string> negativeSemantics = new List<string>();
        List<string> reversingSemantics = new List<string>();
        List<string> unsureSemantics = new List<string>();
        List<string> domainRelated = new List<string>();
        List<string> tempDomain = new List<string>();

        /* List<string> identificationStrings = new List<string>();
         */

        public SemanticsDictionary(string invitation)
        {
            this.invitation = invitation;

            /*identificationStrings = new List<string>
            {
                // Basic
                " me ",
                " i ",

                // Common pairings
                " i'll ",
                " ill ", // unless it refers to feeling ill
                " i'd ",
                " id ", // unless it refers to identification
                " i've ",
                " ive ",
                " i'm ",
                " im ", // unless it refers to instant messaging

                // Groups of us
                " us ", // unless it refers to the country
                " we ", // unless it is short for "whatever"
                " we'll",
                " well ", // unless it refers to something else
                " we'd ",
                " wed ", // unless it refers to a marriage
                " we've ",
                " weve ",
                " we're ",
                " were ", // unless it refers to the past

                // Addition of a party
                " and i ",
                " n i ",
                " and me ",
                " n me ",
                " me and ",
                " me n ",
                " i and ",
                " i n "
            };*/

            actionStrings = new List<string>
            {
                @"am ",
                @"can ",
                @"would ",
                @"will ", // unless it refers to a person or a legal document
                @"do ",
                @"make ",
                @"drive ",
                @"walk ",
                @"heading ", // unless it refers to a direction or the header of a document
                @"plan ",
                @"intend ",
                
            };

            positiveSemantics = new List<string>
            {
                @"yes",
                @"yeah",
                @"yea",
                @"yah",
                @"yep",
                @"sure",
                @"ok",
                @"okay",
                @"okey",
                @"okie",
                @"kk ",
                @"k ",
                @"can+",
                @"will",
                @"good",
                @"down",
                @"bet",
                @"\:\)",
                @"\;\)",
                @"\;D",
                @"\:D",
                @"c\:",
                @"\:\*",
                @"XD",
                @"\:>",
                @"\<3",
                @"affirmative",
                @"totally",
                @"alright",
                @"certainly",
                @"definitely",
                @"right on",
                @"❤",
                @"💋",
                @"💓",
                @"💕",
                @"💖",
                @"💗",
                @"💙",
                @"💚",
                @"💛",
                @"💜",
                @"😀",
                @"😁",
                @"😃",
                @"😄",
                @"😆",
                @"😉",
                @"😊",
                @"😋",
                @"😍",
                @"😎",
                @"😏",
                @"😗",
                @"😘",
                @"😙",
                @"😚",
                @"😸",
                @"😺",
                @"😻",
                @"🙂",
                @"🙆",
                @"🙋",
                @"🙌",
                @"👭",
                @"👫",
                @"👬",
                @"💏",
                @"💑",
                @"👪"
            };

            negativeSemantics = new List<string>
            {
                @"no",
                @"nah",
                @"unfortunately",
                @"sorry",
                @"another time",
                @"but",
                @"tired",
                @"far",
                @"bad",
                @"sucks",
                @"shame",
                @"pity",
                @"\:\(",
                @"D\:",
                @"\:c",
                @"\:<",
                @"\:\,\(",
                @"\:\'\(",
                @"\<\/3",
                @"\<\\3",
                @"work",
                @"forgot",
                @"cancel",
                @"ditch",
                @"pass",
                @"decline",
                @"regret",
                @"💔",
                @"😒",
                @"😓",
                @"😔",
                @"😕",
                @"😖",
                @"😞",
                @"😟",
                @"😠",
                @"😡",
                @"😢",
                @"😣",
                @"😥",
                @"😭",
                @"🙁",
                @"🙅"
            };

            reversingSemantics = new List<string>
            {
                @"not",
                @"n\'t",
                @"cant",
                @"cannot",
                @"wont",
                @"wouldnt"
            };

            unsureSemantics = new List<string>
            {
                @"maybe",
                @"dunno",
                @"perhaps",
                @"idk",
                @"\?",
                @"🤷"
            };

            domainRelated = new List<string>
            {
                @"invite",
                @"invitation",
                @"thank you",
                @"time",
                @"place",
                @"free",
                @"party",
                @"event",
                @"meeting",
                @"meet",
                @"breakfast",
                @"lunch",
                @"dinner",
                @"coffee",
                @"dessert",
                @"today",
                @"schedule",
                @"early",
                @"late",
                @"on time",
                @"plans",
                @"call",
                @"change of plans",
                @"go",
                @"eh"
            };

            foreach (string word in invitation.Split(':'))
            {
                tempDomain.Add(word);
            };
        }

        public int analyzeSemantics()
        {
            int semantic = 0;

            string[] sentences = invitation.Split('.');

            string regexBase = @"\s*";
            MatchCollection matchList;

            foreach (string sentence in sentences)
            {
                int sentenceSemantic = 0;
                int wordCount = invitation.Split(' ').Length;

                for (int i = 0; i < positiveSemantics.Count; i++)
                {
                    matchList = Regex.Matches(invitation, regexBase + positiveSemantics[i]);
                    sentenceSemantic += matchList.Count;
                }

                for (int i = 0; i < reversingSemantics.Count; i++)
                {
                    matchList = Regex.Matches(invitation, regexBase + reversingSemantics[i]);
                    if (matchList.Count > 0)
                    {
                        sentenceSemantic *= -1 * matchList.Count;
                    }
                }

                for (int i = 0; i < negativeSemantics.Count; i++)
                {
                    matchList = Regex.Matches(invitation, regexBase + negativeSemantics[i]);
                    sentenceSemantic -= matchList.Count;
                }

                for (int i = 0; i < unsureSemantics.Count; i++)
                {
                    matchList = Regex.Matches(invitation, regexBase + unsureSemantics[i]);
                    if (matchList.Count > 0)
                    {
                        sentenceSemantic = 0;
                    }
                }

                if (sentenceSemantic > 0)
                {
                    semantic++;
                }
                else if(sentenceSemantic < 0)
                {
                    semantic--;
                }
            }

            return semantic;
        }
    }
}
