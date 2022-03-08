// functions
Array.prototype.sum = Array.prototype.sum || function () {
    return this.reduce(function (sum, a) { return sum + Number(a) }, 0);
}
Array.prototype.average = Array.prototype.average || function () {
    return this.sum() / (this.length || 1);
}