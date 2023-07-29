using QueryPressure.Core;

namespace QueryPressure.Postgres.App;

public record PostgresProviderInfo()
  : ProviderInfo("Postgres", new[] { "pgsql", "sql" }, Script)
{
  private const string Script = """
-- Sample
 SELECT
    t.relname AS table_name,
    a.attname AS column_name,
    CASE
      WHEN a.attnotnull THEN 'NOT NULL'
      ELSE ''
    END AS constraints,
    i.relname AS index_name
  FROM
    pg_class t
  JOIN
    pg_attribute a ON t.oid = a.attrelid
  LEFT JOIN
    pg_index ix ON t.oid = ix.indrelid AND a.attnum = ANY(ix.indkey)
  LEFT JOIN
    pg_class i ON ix.indexrelid = i.oid
  WHERE
    t.relkind = 'r'  -- Only regular tables
    AND t.relname NOT LIKE 'pg_%'  -- Exclude system tables
    AND t.relname NOT LIKE 'sql_%'  -- Exclude SQL-standard tables
  ORDER BY
    t.relname, a.attnum;
""";
}
