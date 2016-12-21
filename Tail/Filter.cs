using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tail
{

    interface IFilter
    {
        FilterResult IsMatch(string input);
    }

    class FilterResult
    {
        public bool Success { get; set; }
        public string Identifier { get; set; }
    }

    class RegexFilter : IFilter
    {
        private readonly string _identifier;
        private Regex _regex;

        public RegexFilter(string regExp) : this(regExp, regExp)
        {
        }

        public RegexFilter(string regExp, string identifier)
        {
            _identifier = identifier;
            _regex = new Regex(regExp);
        }

        public FilterResult IsMatch(string input)
        {
            if (_regex.IsMatch(input))
            {
                return new FilterResult
                {
                    Success = true,
                    Identifier = _identifier
                };
            }
            else
            {
                return new FilterResult
                {
                    Success = false,
                    Identifier = _identifier
                };
            }
        }
    }

    class CombinedFilter : IFilter
    {
        private readonly IFilter[] _filters;

        public CombinedFilter(params IFilter[] filters )
        {
            _filters = filters;
        }


        public FilterResult IsMatch(string input)
        {
            foreach (var filter in _filters)
            {
                var result = filter.IsMatch(input);

                if (result.Success)
                {
                    return result;
                }
            }

            return new FilterResult
            {
                Success = false
            };
        }
    }
}
