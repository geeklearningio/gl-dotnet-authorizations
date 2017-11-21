﻿namespace GeekLearning.Authorizations.Model.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ScopeRights
    {
        public ScopeRights()
        {
        }

        public ScopeRights(Guid principalId, string scopeName, List<Right> rightsOnScope, List<Right> rightsUnderScope)
        {
            this.PrincipalId = principalId;
            this.ScopeName = scopeName;

            this.RightsOnScope = ComputeRights(principalId, scopeName, rightsOnScope);
            rightsOnScope.AddRange(rightsUnderScope);
            this.RightsUnderScope = ComputeRights(principalId, scopeName, rightsOnScope);
        }

        public Guid PrincipalId { get; set; }

        public string ScopeName { get; set; }

        public IReadOnlyDictionary<string, Right> RightsOnScope { get; set; }

        public IReadOnlyDictionary<string, Right> RightsUnderScope { get; set; }

        public bool HasAnyExplicitRight 
            => this.RightsOnScope.Values.Any(r => r.IsExplicit);

        public bool HasAnyRightUnder 
            => this.RightsUnderScope.Values.Any();

        public bool HasRight(string right) 
            => this.RightsOnScope.ContainsKey(right);

        public bool HasInheritedRight(string right) 
            => this.RightsOnScope.ContainsKey(right) && !this.RightsOnScope[right].IsExplicit;

        public bool HasExplicitRight(string right) 
            => this.RightsOnScope.ContainsKey(right) && this.RightsOnScope[right].IsExplicit;

        public bool HasRightUnder(string right) 
            => this.RightsUnderScope.ContainsKey(right);

        private static Dictionary<string, Right> ComputeRights(Guid principalId, string scopeName, List<Right> rights)
        {
            if (rights != null && rights.Count > 0)
            {
                var rr = rights
                    .GroupBy(r => r.RightName)
                    .ToDictionary(rg => rg.Key, rg => new Right(principalId, scopeName, rg.Key, rg.Any(r => r.IsExplicit)));
            
                return rr;
            }

            return new Dictionary<string, Right>();
        }
    }
}