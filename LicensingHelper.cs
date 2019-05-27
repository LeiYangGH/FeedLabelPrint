using Portable.Licensing;
using Portable.Licensing.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FeedLabelPrint
{
    public sealed class LicensingHelper
    {
        public static void CreateLicense()
        {
            var license = License.New()
            .WithUniqueIdentifier(Guid.NewGuid())
            .As(LicenseType.Trial)
            .ExpiresAt(DateTime.Now.AddDays(30))
            .WithAdditionalAttributes(new Dictionary<string, string>
                                  {
                                                  {Constants.MacAddress, Constants.TargetMacAddress},
                                  })
.CreateAndSignWithPrivateKey("MIICITAjBgoqhkiG9w0BDAEDMBUEEJ3s180QFCySSMoIp0zMlSACAQoEggH4N/mOlMyfKCVkpePHkB930Nzkjn6B4AByE/d5/PWdlwqlvzOG5CgQ20PZPmg4cLdvrOnkkIjaYOEIbaSlzZZess5SdejSwfbhwfR9jf/OcYBj+pWOHeV76+x6a6dyi5kyaO3XJB5SxiFtAX8DijSvf/Fg7W8dQjdUO0hiKcuMquvkAQ1UQtvv4Fq5L9mQJEGZqxMEY8fzCgnUGVlk3wgwBGQvtoNtAjtmo2fDIuJGvV/aoy3rK+fYf8ftDnO4bbBZInpTY9Gs479P+v7EgO19OzlRDwiI739xITNIhuqhB8j0SAvpfQxcXwCrXxAKqgjqjdpXXa6TzXKoriBBZqx9Bks9xN0xbXclZV5EKwkA+IiVJGTQ+Ie2c4/Raj59EVCwlUyxLlA+vWjZBAzjwNMfeUzDwNjBGFEqARbjWfyWMW/VFOaSOzGXso7CBWslsKh+IbwC4V/6jzTwZzKJzm9JmUlegQ8O8k9HkI6P8PXdDAxLh5jZfEH+1eavie43n24/Ue49Rr4SWFC0VuGT8KNE+YrIDxdMvL7npZ6gxtY0phmT7+O/nWYTIL8YV3+fC2h0kdldgGh5XuhP7/rQ0qiZTnjaHLwU5gx/RtBUKC710qYa5Et/249jXUwMoJ4pqsstKaRO+2A0g/FcZDYxueCrkgZOSibYPT24", Constants.FeedLabelPrint);
            File.WriteAllText("License.xml", license.ToString(), Encoding.UTF8);
        }



        public static IEnumerable<IValidationFailure> ValidateLicense(string licenseFile)
        {

            var license1 = License.Load(File.ReadAllText(licenseFile, Encoding.UTF8));
            var validationFailures1 = license1.Validate()
                                .ExpirationDate()
                                    .When(lic => lic.Type == LicenseType.Trial)
                                .And()
                                .Signature(Constants.PublicKey)
                                .AssertValidLicense();
            var validationFailures2 = license1.Validate().AssertThat(lic => lic.AdditionalAttributes.Get(Constants.MacAddress) == GetMacAddress(),
                new InvalidMac()
                {
                    Message = "Invalid Mac",
                    HowToResolve = "Contact administrator"
                }).AssertValidLicense();

            return validationFailures1.Union(validationFailures2).Distinct();

        }


        public static string GetMacAddress()
        {

            return NetworkInterface
              .GetAllNetworkInterfaces()
              .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
              .Select(nic => nic.GetPhysicalAddress().ToString())
              .FirstOrDefault();


        }

        class InvalidMac : IValidationFailure
        {
            public string Message { get; set; }
            public string HowToResolve { get; set; }
        }
    }

}
