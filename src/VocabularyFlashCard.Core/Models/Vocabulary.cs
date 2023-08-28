using System.ComponentModel;

namespace VocabularyFlashCard.Core.Models
{
	[Table("Vocabulary")]
	public class Vocabulary
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int VocabularyId { get; set; }

		[Required, StringLength(50)]
		public string Word { get; set; } = string.Empty;

		public string Definition { get; set; } = string.Empty;

		public short NumRepeat { get; set; }

		public byte GroupId { get; set; }

		[Column(TypeName = "DateTime2(2)")]  // DateTime2 with (2) precision with 6 bytes for storage
		public DateTime LastRead { get; set; }

		[DefaultValue(false)]
		public bool? Marked { get; set; } = false;

		// public IEnumerable<VocabularyMedia> VocabularyMedias { get; set; } = Enumerable.Empty<VocabularyMedia>();
		public IEnumerable<VocabularyMedia>? VocabularyMedias { get; set; } = null;
	}
}