using SmartCode.Generator.Entity;
using SmartCode.WordsConverter;

namespace SmartCode.Generator.RazorTemplates.Contributions {

    public static class Common {

        public static string ToPath(string tableName) {
            var words = tableName.ToLowerInvariant().Split('_');
            var converter = new StrikeThroughConverter();
            return converter.Convert(words);
        }

        public static string ToClassName(string tableName) {
            var words = tableName.Split('_');
            var converter = new PascalCaseSingularConverter();
            var className = converter.Convert(words);
            return className;
        }

        public static string ConvertCsType(Column column) {
            if (string.IsNullOrEmpty(column.LanguageType)) {
                return "NAType";
            }
            if (!column.IsNullable) {
                return column.LanguageType;
            }
            if (column.LanguageType.Contains("[]") || column.LanguageType.ToLower() == "string") {
                return column.LanguageType;
            }
            return $"{column.LanguageType}?";
        }

        public static string ConvertCsModelType(Column col) {
            if (col.LanguageType.ToLower() == "long" && col.Name.EndsWith("_id")) {
                return "string";
            }
            return ConvertCsType(col);
        }

        public static string GetGeneratorClass(Column col) {
            if (col.LanguageType == "int") {
                return "trigger-identity";
            }
            if (col.LanguageType == "long") {
                return "trigger-identity";
            }
            return "assigned";
        }

        public static string GetHbmType(string langType) {
            if (langType == "byte[]") {
                return "binary";
            }
            return langType.ToLower();
        }

        public static string ConvertTsType(
            Column col
        ) {
            if (col.LanguageType.ToLower() == "number"
                && (col.Name.Equals("id") || col.Name.EndsWith("_id"))) {
                return "string";
            }
            return col.LanguageType;
        }

        public static string ToNgSelector(
            string tableName
        ) {
            var words = tableName.Split('_');
            var converter = new StrikeThroughConverter();
            var selector = converter.Convert(words);
            if (!selector.StartsWith("app-")) {
                selector = "app-" + selector;
            }
            return NamingUtil.ToSingular(selector);
        }

    }

}
