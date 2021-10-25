export const getTime = (date: Date = new Date()): string => {
    return date.getHours().toString() + ':' + date.getMinutes().toString() + ':' + date.getSeconds().toString()
}