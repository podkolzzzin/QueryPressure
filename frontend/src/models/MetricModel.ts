export interface MetricModel {
  name: MetricNames;
  value: any;
}

export enum MetricNames {
  LiveAverage = 'live-average',
  LiveRequestCount = 'live-request-count'
}