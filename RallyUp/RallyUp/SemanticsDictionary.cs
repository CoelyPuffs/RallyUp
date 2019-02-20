using System;
using System.Collections.Generic;
using System.Text;

namespace RallyUp
{
    class SemanticsDictionary
    {
        List<string> positiveSemantics = new List<string>();
        List<string> negativeSemantics = new List<string>();
        List<string> domainRelated = new List<string>();

        SemanticsDictionary()
        {
            positiveSemantics = new List<string>
            {
                "yes",
                "yeah",
                "yea",
                "yep",
                "sure",
                "ok",
                "okay",
                "okey",
                "okie",
                "kk",
                "k",
                "can",
                "will",
                "good",
                "down",
                "bet",
                ":)",
                ":D",
                "c:",
                "k",
                "affirmative",
                "totally",
                "alright",
                "certainly",
                "definitely"
            };

            negativeSemantics = new List<string>
            {
                "no",
                "nah",
                "can't",
                "cant",
                "cannot",
                "not",
                "unfortunately",
                "sorry",
                "another time",
                "but",
                "tired",
                "far",
                ":(",
                "D:",
                ":c",
                "work",
                "wont",
                "won't",
                "wont",
                "forgot",
                "cancel",
                "ditch",
                "pass",
                "decline",
                "regret"
            };

            domainRelated = new List<string>
            {
                "invite",
                "invitation",
                "thank you",
                "time",
                "place",
                "free",
                "party",
                "event",
                "meeting",
                "meet",
                "breakfast",
                "lunch",
                "dinner",
                "coffee",
                "dessert",
                "today",
                "schedule",
                "early",
                "late",
                "on time",
                "plans",
                "call",
                "change of plans"
            };
        }
    }
}
