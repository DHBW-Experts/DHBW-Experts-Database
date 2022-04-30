using DatabaseAPI.Model;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace DatabaseAPI.Controllers {
    public static class Functions {

        public static int generateVerificationCode() {
            var rand = new Random();
            return rand.Next(999999) + 1;
        }
    }
}
