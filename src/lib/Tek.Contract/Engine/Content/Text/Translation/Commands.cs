using System;
using System.Collections.Generic;

using Tek.Contract;

namespace Tek.Contract.Engine
{
    public class CreateTranslation
    {
        public Guid TranslationId { get; set; }

        public string TranslationText { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public class ModifyTranslation
    {
        public Guid TranslationId { get; set; }

        public string TranslationText { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public class DeleteTranslation
    {
        public Guid TranslationId { get; set; }
    }

    public class ExportTranslations
    {
        public ICollection<CreateTranslation> Items { get; set; }
    }

    public class ImportTranslations
    {
        public ICollection<CreateTranslation> Items { get; set; }
    }

    public class PurgeTranslations
    {
        public ICollection<DeleteTranslation> Items { get; set; }
    }
}