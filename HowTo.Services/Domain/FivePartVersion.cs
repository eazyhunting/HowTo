using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowTo.Services.Domain
{
    public sealed class FivePartVersion
    {
        public int Major { get; set; } = 0;
        public int Minor { get; set; } = 0;
        public int Revision { get; set; } = 0;
        public int File { get; set; } = 0;
        public int Informational { get; set; } = 0;

        public FivePartVersion(int major, int minor, int revision, int file, int informational)
        {
            Major = major;
            Minor = minor;
            Revision = revision;
            File = file;
            Informational = informational;
        }

        public FivePartVersion(string version)
        {
            var parts = version.Split('.');

            if (!validatePart(parts.ElementAtOrDefault(0) ?? string.Empty, out var major))
            {
                throw new ArgumentException("Version info has not been provided in the correct format.");
            }
            Major = major;

            if (!validatePart(parts.ElementAtOrDefault(1) ?? string.Empty, out var minor))
            {
                throw new ArgumentException("Version info has not been provided in the correct format.");
            }
            Minor = minor;

            if (!validatePart(parts.ElementAtOrDefault(2) ?? string.Empty, out var revision))
            {
                throw new ArgumentException("Version info has not been provided in the correct format.");
            }
            Revision = revision;

            if (!validatePart(parts.ElementAtOrDefault(1) ?? string.Empty, out var file))
            {
                throw new ArgumentException("Version info has not been provided in the correct format.");
            }
            File = file;

            if (!validatePart(parts.ElementAtOrDefault(1) ?? string.Empty, out var info))
            {
                throw new ArgumentException("Version info has not been provided in the correct format.");
            }
            Informational = info;            
        }
        public int CompareTo(FivePartVersion? other)
        {
            if (other == null) throw new ArgumentException("Please specify a version to compare");

            if (ReferenceEquals(this, other)) return 0;

            if (this.Major > other.Major) return 1;
            if (this.Major < other.Major) return -1;

            if (this.Minor > other.Minor) return 1;
            if (this.Minor < other.Minor) return -1;

            if (this.Revision > other.Revision) return 1;
            if (this.Revision < other.Revision) return -1;

            if (this.File > other.File) return 1;
            if (this.File < other.File) return -1;

            if (this.Informational > other.Informational) return 1;
            if (this.Informational < other.Informational) return -1;

            return 0;
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Revision}.{File}.{Informational}";
        }

        private bool validatePart(string part, out int parsedValue)
        {
            part = string.IsNullOrEmpty(part) ? 0.ToString()
                : int.TryParse(part, out parsedValue) ? parsedValue.ToString() : "Invalid Value";

            if (!int.TryParse(part, out parsedValue))
            {
                return false;
            }
            return true;
        }
    }
}
