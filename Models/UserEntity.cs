using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PocStubAppo1.Models
{
    [Table("user")]
    [Serializable]
    public class UserEntity
    {
        // 主キー
        [Key]
        // インクリメント
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // カラム名
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("age")]
        public int Age { get; set; }

        [Column("hobby")]
        public string Hobby { get; set; }
    }
}
