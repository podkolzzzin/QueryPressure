# QueryPressure

The tool for load testing of...

## Development

Build sln file: 
```console
slngen --solutionfile src\QueryPressure.sln --folders true --collapsefolders true --nologo --launch false
```


## Features

1. ❌ Multi-tab
2. ✔️ CLI, CI support
3. ❌ UI client
4. ✔️ Plugins
5. Databases support
    - ✔️ SQLServer
    - ✔️ Postgres
    - ✔️ MySQL/MariaDB
    - ❌ CosmosDB
    - ❌ Mongo
    - ❌ Elastic Search
    - ✔️ Redis
    - ✔️ Expandable with plugins
6. Cross-platform support
    - ❌ MacOC
    - ❌ Linux
    - ✔️ Windows
7. Metrics
    - ❌ Distribution
    - ✔️ Avg
    - ❌ Med
    - ❌ Std. Dev
    - ❌ Confidence interval
    - ❌ Expandable with plugins
8. Load Profiles
    - Limitations:
        - ✔️ Time
        - ✔️ Number queries
        - ✔️ Till first error
    - Load characteristics:
        - ✔️ Sequential
            - ✔️ With Delay
        - ✔️ Target Throughput
            - ❌ Increasing load
                - ❌ Delta/Time
        - ✔️ Limited Concurrency
            - ✔️ With Delay
    - ✔️ Expandable with plugins
9. Reporting, export
    - ❌ CSV Excel
    - ❌ XML
    - ❌ JSON
10. Graphics/Dashboards

Competitors: SQLQueryStress, NBomber, BenchmarkDotNet 
