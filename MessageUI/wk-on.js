
var ws, item, buffers = [], uploadIndex = 0, wsSessionId;

onmessage = function (e) {
    var data = e.data;
    console.log('WK[' + data.id + '].1 = ', data);
    item = data;
    //postMessage('WK');
};

/*===[CONFIG_SERVER_UPLOAD_IMAGE_HERE]!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!===*/
ws = new WebSocket("ws://222.252.22.118:9099/");
var size = 65536; // max limit are 65536 64kB
/*===[CONFIG_SERVER_UPLOAD_IMAGE_HERE]!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!===*/

ws.binaryType = 'arraybuffer';
//ws.binaryType = 'blob';
ws.onopen = function () {
    postMessage({ Code: 'SOCKET_OPEN' });
    //ws.send('FOLDER.ADD:folder1');
    //ws.send('FILE.ADD:' + data.name);
    //ws.send(rawData);
    //postMessage({ Code: 'SOCKET_SENDING_FILE' });

    var a = new Uint8Array([8, 6, 7, 3, 5, 9.0]);
    console.log('WORKER.OPEN.SOCKET: ', a.buffer);
    //ws.send(a.buffer);

    var reader = new FileReader();
    reader.loadend = function () { }
    reader.onload = function (er) {
        var rawData = er.target.result; // ArrayBuffer
        var uint8Array = new Uint8Array(rawData);

        //console.log('WK.rawData = ', rawData);
        //console.log('WK.uint8Array = ', uint8Array);

        var len = uint8Array.length;
        var page = Math.round(len / size);
        if (len % size != 0) page++;
        var blob, min, max;

        //ws.send('PAGE=' + page);

        for (var i = 0; i < page; i++) {
            min = i * size;
            max = (i + 1) * size;
            if (max > len) max = len;

            blob = uint8Array.slice(min, max); 
            buffers.push(blob);
        }

        console.log('WORKER.BUFFER: '+ item.id + '.' + item.name + ' === ' + buffers.length);
        //-> send info of the file to begin buffering via socket
        ws.send(JSON.stringify({ Id: item.id, folder: '', name: item.name, size: item.size, type: item.type }));
    }
    reader.readAsArrayBuffer(item.file);

};
ws.onmessage = function (evt) {
    var data = evt.data;
    //console.log('WK ws.msg = ', uploadIndex, data);

    switch (data) { 
        case 'SOCKET_BUFFERING_START':
            //-> begin buffering first packet
            //var a = new Uint8Array([8, 6, 7, 3, 5, 9.0]);
            console.log('WORKER.SOCKET_BUFFERING_START: ', buffers[0]);
            //ws.send(a.buffer);
            ws.send(buffers[0]);
            postMessage({ Id: item.id, Name: item.name, Code: 'SOCKET_SENDING', PageTotal: buffers.length, Index: 1 });
            uploadIndex++;
            break;
        case 'SOCKET_BUFFERING':
            //-> begin buffering next packet
            postMessage({ Id: item.id, Name: item.name, Code: 'SOCKET_SENDING', PageTotal: buffers.length, Index: uploadIndex });
            ws.send(buffers[uploadIndex]);
            uploadIndex++;
            if (uploadIndex == buffers.length) {
                ws.send('SENDING_COMPLETE');
                //postMessage({ Code: 'SOCKET_SENDING_COMPLETE', PageTotal: buffers.length, Index: uploadIndex });
            }
            break;
        default:
            if (data.length > 1 && data[0] == '{' && data[data.length - 1] == '}') {
                var fiNew = JSON.parse(data);
                if (fiNew.Code == 'FILE_CHANGE_NAME') {
                    item.nameNew = fiNew.nameNew;
                }
                console.log('WORKER.' + fiNew.Code + ': ' + fiNew.nameNew);
                postMessage(fiNew);
            } else {
                postMessage({ Id: item.id, Name: item.name, Code: 'SOCKET_MSG', Data: data });
            }
            break;
    }
};
ws.onerror = function (evt) {
    postMessage({ Code: 'SOCKET_ERROR' });
};
ws.onclose = function () {
    postMessage({ Code: 'SOCKET_CLOSE' });
};