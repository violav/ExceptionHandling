using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vi.exceptions
{
    internal enum RequestRejectionReason
    {
        TlsOverHttpError,
        UnrecognizedHTTPVersion,
        InvalidRequestLine,
        InvalidRequestHeader,
        InvalidRequestHeadersNoCRLF,
        MalformedRequestInvalidHeaders,
        InvalidContentLength,
        MultipleContentLengths,
        UnexpectedEndOfRequestContent,
        BadChunkSuffix,
        BadChunkSizeData,
        ChunkedRequestIncomplete,
        InvalidRequestTarget,
        InvalidCharactersInHeaderName,
        RequestLineTooLong,
        HeadersExceedMaxTotalSize,
        TooManyHeaders,
        RequestBodyTooLarge,
        RequestHeadersTimeout,
        RequestBodyTimeout,
        FinalTransferCodingNotChunked,
        LengthRequired,
        LengthRequiredHttp10,
        OptionsMethodRequired,
        ConnectMethodRequired,
        MissingHostHeader,
        MultipleHostHeaders,
        InvalidHostHeader,
        UpgradeRequestCannotHavePayload,
        RequestBodyExceedsContentLength
    }
}
