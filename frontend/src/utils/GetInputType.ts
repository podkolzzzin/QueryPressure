export function getInputType(argumentType: string): string {
  let result;
  switch (argumentType) {
    case 'int':
      result = 'number';
      break;
    case 'TimeSpan':
      result = 'time';
      break;
    default:
      result = 'text';
  }

  return result;
}
