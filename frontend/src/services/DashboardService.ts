import { HubConnectionBuilder } from '@microsoft/signalr';

import { MetricModel } from '@/models/MetricModel';

const connection = new HubConnectionBuilder()
  .withAutomaticReconnect()
  .withUrl('/ws/dashboard')
  .build();

connection.start();

export const DashboardService = {
  onMetricsReceived(func: (message: MetricModel[]) => void) {
    connection.on('DashboardHub.metrics', func);
  }
};