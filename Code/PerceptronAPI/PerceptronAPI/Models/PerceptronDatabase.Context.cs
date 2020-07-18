﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PerceptronAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PerceptronDatabaseEntities : DbContext
    {
        public PerceptronDatabaseEntities()
            : base("name=PerceptronDatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ExecutionTime> ExecutionTimes { get; set; }
        public virtual DbSet<PeakListData> PeakListDatas { get; set; }
        public virtual DbSet<PtmFixedModification> PtmFixedModifications { get; set; }
        public virtual DbSet<PtmVariableModification> PtmVariableModifications { get; set; }
        public virtual DbSet<ResultInsilicoMatchLeft> ResultInsilicoMatchLefts { get; set; }
        public virtual DbSet<ResultInsilicoMatchRight> ResultInsilicoMatchRights { get; set; }
        public virtual DbSet<ResultPtmSite> ResultPtmSites { get; set; }
        public virtual DbSet<ResultsDownloadable> ResultsDownloadables { get; set; }
        public virtual DbSet<SearchFile> SearchFiles { get; set; }
        public virtual DbSet<SearchParameter> SearchParameters { get; set; }
        public virtual DbSet<SearchQuery> SearchQueries { get; set; }
        public virtual DbSet<SearchResult> SearchResults { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
    }
}
