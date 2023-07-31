import { ArgumentSection } from '@/models';
import { UrlService } from '@/services/UrlService';

export function applyArgument(type: string, arg: ArgumentSection, val: string) {
    arg.value = val;
    UrlService.set(`${type}-${arg.name}`, val);
}

export function loadArgument(type: string, arg: ArgumentSection) {
    arg.value = UrlService.get(`${type}-${arg.name}`) ?? undefined;
}